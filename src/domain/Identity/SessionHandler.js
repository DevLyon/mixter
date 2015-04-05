var SessionProjection = require('./SessionProjection');

var SessionHandler = function SessionHandler(sessionsRepository){
    var saveProjection = function saveProjection(event, isEnabled){
        var projection = new SessionProjection.create(event.sessionId, event.userIdentityId, isEnabled);
        sessionsRepository.save(projection);
    };

    this.handleUserConnected = function(event){
        saveProjection(event, SessionProjection.SessionEnabled);
    };

    this.handleUserDisconnected = function(event){
        saveProjection(event, SessionProjection.SessionDisabled);
    };
};

exports.create = function(sessionsRepository){
    return new SessionHandler(sessionsRepository);
};