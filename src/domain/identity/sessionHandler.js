var sessionProjection = require('./sessionProjection');
var session = require('./session');

var SessionHandler = function SessionHandler(sessionsRepository){
    var saveProjection = function saveProjection(event, isEnabled){
        var projection = new sessionProjection.create(event.sessionId, event.userIdentityId, isEnabled);
        sessionsRepository.save(projection);
    };

    this.register = function register(eventPublisher) {
        eventPublisher.on(session.UserConnected, function(event){
            saveProjection(event, sessionProjection.SessionEnabled);
        }).on(session.UserDisconnected, function(event){
            saveProjection(event, sessionProjection.SessionDisabled);
        });
    };
};

exports.create = function(sessionsRepository){
    return new SessionHandler(sessionsRepository);
};