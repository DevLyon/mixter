var UserIdentity = require('./domain/identity/UserIdentity');
var EventPublisher = require('./infrastructure/EventPublisher');
var UserIdentityId = require('./domain/Identity/UserIdentity').UserIdentityId;

var eventsStore = require('./infrastructure/EventsStore').create();

var createPublishEvent = function createPublishEvent(eventsStore) {
    var eventPublisher = EventPublisher.create();
    eventPublisher.onAny(eventsStore.store);

    return eventPublisher.publish;
};

var publishEvent = createPublishEvent(eventsStore);

var registerUser = function registerUser(req, res) {
    var email = req.body.email;

    UserIdentity.register(publishEvent, email);

    res.status(201).send({
        id: new UserIdentityId(email),
        url: '/api/identity/userIdentities/' + encodeURIComponent(email),
        logIn: '/api/identity/userIdentities/' + encodeURIComponent(email) + '/logIn'
    });
};

exports.registerRoutes = function registerRoutes(app){
    app.post('/api/identity/userIdentities/register', registerUser);
};