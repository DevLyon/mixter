var userIdentity = require('./domain/identity/userIdentity');
var SessionId = require('./domain/identity/session').SessionId;
var message = require('./domain/core/message');
var UserId = require('./domain/userId').UserId;
var sessionHandler = require('./domain/identity/sessionHandler');
var updateTimeline = require('./domain/core/updateTimeline');
var createEventPublisher = require('./infrastructure/eventPublisher').create;

var eventsStore = require('./infrastructure/eventsStore').create();
var userIdentitiesRepository = require('./infrastructure/userIdentitiesRepository').create(eventsStore);
var sessionsRepository = require('./infrastructure/sessionsRepository').create(eventsStore);
var timelineMessagesRepository = require('./infrastructure/timelineMessageRepository').create();
var messagesRepository = require('./infrastructure/messagesRepository').create(eventsStore);

var createPublishEvent = function createPublishEvent(eventsStore) {
    var eventPublisher = createEventPublisher();
    eventPublisher.onAny(eventsStore.store);
    sessionHandler.create(sessionsRepository).register(eventPublisher);
    updateTimeline.create(timelineMessagesRepository).register(eventPublisher);

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
    var sessionId = new SessionId(req.params.id);

    var session = sessionsRepository.getSession(sessionId);

    session.logOut(publishEvent);

    res.status(200).send('User disconnected');
};

var quackMessage = function quackMessage(req, res){
    var author = new UserId(req.body.author);
    var content = req.body.content;

    var messageId = message.quack(publishEvent, author, content);

    res.status(201).send({
        id: messageId,
        url: '/api/core/messages/' + encodeURIComponent(messageId.id)
    });
};

var deleteMessage = function deleteMessage(req, res){
    var sessionId = new SessionId(req.body.sessionId);

    var deleter = sessionsRepository.getUserIdOfSession(sessionId);
    if(!deleter){
        res.status(403).send('Invalid session');
        return;
    }

    var messageId = new message.MessageId(req.params.id);
    var messageToDeleted = messagesRepository.getMessage(messageId);

    messageToDeleted.delete(publishEvent, deleter);

    res.status(200).send('Message deleted');
};

var getTimelineMessages = function getTimelineMessages(req, res) {
    var owner = new UserId(req.params.owner);

    var messages = timelineMessagesRepository.getMessageOfUser(owner);

    res.status(200).send(messages);
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

    app.post('/api/core/messages/quack', manageError(quackMessage));
    app.delete('/api/core/messages/:id', manageError(deleteMessage));
    app.get('/api/core/timelineMessages/:owner', manageError(getTimelineMessages));
};
