var UserIdentity = require('./domain/identity/UserIdentity');
var SessionHandler = require('./domain/identity/SessionHandler');
var EventPublisher = require('./infrastructure/EventPublisher');
var UserIdentityId = require('./domain/Identity/UserIdentity').UserIdentityId;

var eventsStore = require('./infrastructure/EventsStore').create();

var createPublishEvent = function createPublishEvent(eventsStore) {
    var eventPublisher = EventPublisher.create();
    eventPublisher.onAny(eventsStore.store);
    SessionHandler.create().register(eventPublisher);

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
};
