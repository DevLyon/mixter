var userIdentity = require('./domain/identity/userIdentity');
var eventPublisherModule = require('./infrastructure/eventPublisher');
var sessionHandler = require('./domain/identity/sessionHandler');
var UserIdentityId = require('./domain/identity/userIdentity').UserIdentityId;

var eventsStore = require('./infrastructure/eventsStore').create();

var createPublishEvent = function createPublishEvent(eventsStore) {
    var eventPublisher = eventPublisherModule.create();
    eventPublisher.onAny(eventsStore.store);
    sessionHandler.create().register(eventPublisher);

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
