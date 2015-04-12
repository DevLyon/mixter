var userIdentity = require('./domain/identity/userIdentity');
var Session = require('./domain/identity/session');
var UserId = require('./domain/userId').UserId;
var sessionHandler = require('./domain/identity/sessionHandler');
var eventPublisherModule = require('./infrastructure/eventPublisher');

var eventsStore = require('./infrastructure/eventsStore').create();
var userIdentitiesRepository = require('./infrastructure/userIdentitiesRepository').create(eventsStore);
var sessionsRepository = require('./infrastructure/sessionsRepository').create(eventsStore);

var createPublishEvent = function createPublishEvent(eventsStore) {
    var eventPublisher = eventPublisherModule.create();
    eventPublisher.onAny(eventsStore.store);
    sessionHandler.create(sessionsRepository).register(eventPublisher);

    return eventPublisher.publish;
};

var publishEvent = createPublishEvent(eventsStore);

var registerUser = function registerUser(req, res) {
    var email = req.body.email;

    userIdentity.register(publishEvent, email);

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
};
