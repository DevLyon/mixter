var userIdentity = require('./domain/identity/userIdentity');
var eventPublisherModule = require('./infrastructure/eventPublisher');
var UserIdentityId = require('./domain/identity/userIdentity').UserIdentityId;

var eventsStore = require('./infrastructure/eventsStore').create();

var createPublishEvent = function createPublishEvent(eventsStore) {
    var eventPublisher = eventPublisherModule.create();
    eventPublisher.onAny(eventsStore.store);

    return eventPublisher.publish;
};

var publishEvent = createPublishEvent(eventsStore);

var registerUser = function registerUser(req, res) {
    var email = req.body.email;

    userIdentity.register(publishEvent, email);

    res.status(201).send({
        id: new UserIdentityId(email),
        url: '/api/identity/userIdentities/' + encodeURIComponent(email),
        logIn: '/api/identity/userIdentities/' + encodeURIComponent(email) + '/logIn'
    });
};

exports.registerRoutes = function registerRoutes(app){
    app.post('/api/identity/userIdentities/register', registerUser);
};