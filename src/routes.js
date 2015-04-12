var UserIdentity = require('./domain/identity/UserIdentity');
var Session = require('./domain/identity/Session');
var Message = require('./domain/core/Message');
var UserId = require('./domain/UserId').UserId;
var SessionHandler = require('./domain/identity/SessionHandler');
var EventPublisher = require('./infrastructure/EventPublisher');

var eventsStore = require('./infrastructure/EventsStore').create();
var userIdentitiesRepository = require('./infrastructure/UserIdentitiesRepository').create(eventsStore);
var sessionsRepository = require('./infrastructure/SessionsRepository').create(eventsStore);

var createPublishEvent = function createPublishEvent(eventsStore) {
    var eventPublisher = EventPublisher.create();
    eventPublisher.onAny(eventsStore.store);
    SessionHandler.create(sessionsRepository).register(eventPublisher);

    return eventPublisher.publish;
};

var publishEvent = createPublishEvent(eventsStore);

var registerUser = function registerUser(req, res) {
    var email = req.body.email;

    UserIdentity.register(publishEvent, email);

    res.status(201).send({
        id: new UserId(email),
        url: '/api/identity/userIdentities/' + encodeURIComponent(email),
        logIn: '/api/identity/userIdentities/' + encodeURIComponent(email) + '/logIn'
    });
};

var logInUser = function logInUser(req, res){
    var userId = new UserId(req.params.id);

    var userIdentity = userIdentitiesRepository.getUserIdentity(userId);

    var sessionId = userIdentity.logIn(publishEvent);

    res.status(201).send({
        id: sessionId,
        url: '/api/identity/sessions/' + encodeURIComponent(sessionId.id)
    });
};

var logOutUser = function logOutUser(req, res){
    var sessionId = new Session.SessionId(req.params.id);

    var session = sessionsRepository.getSession(sessionId);

    session.logOut(publishEvent);

    res.status(200).send('User disconnected');
};

var publishMessage = function publishMessage(req, res){
    var author = new UserId(req.body.author);
    var content = req.body.content;

    var messageId = Message.publish(publishEvent, author, content);

    res.status(201).send({
        id: messageId,
        url: '/api/core/messages/' + encodeURIComponent(messageId.id)
    });
};

var manageError = function manageError(action){
    return function(req, res){
        try {
            action(req, res);
        } catch(e){
            if(e.constructor) {
                var errorName = e.constructor.name;

                console.log('error: ' + errorName);
                console.log(e);

                res.status(400).send({
                    errorName: errorName,
                    error: e
                });

                return;
            }

            throw e;
        }
    };
};

exports.registerRoutes = function registerRoutes(app){
    app.post('/api/identity/userIdentities/register', manageError(registerUser));
    app.post('/api/identity/userIdentities/:id/logIn', manageError(logInUser));
    app.delete('/api/identity/sessions/:id', manageError(logOutUser));

    app.post('/api/core/messages/publish', manageError(publishMessage));
};
