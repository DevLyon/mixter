var sessionProjection = require('./sessionProjection');

var SessionHandler = function SessionHandler(sessionsRepository){
    var saveProjection = function saveProjection(event, isEnabled){
        var projection = new sessionProjection.create(event.sessionId, event.userIdentityId, isEnabled);
        sessionsRepository.save(projection);
    };

    this.handleUserConnected = function(event){
        saveProjection(event, sessionProjection.SessionEnabled);
    };

    this.handleUserDisconnected = function(event){
        saveProjection(event, sessionProjection.SessionDisabled);
    };
};

exports.create = function(sessionsRepository){
    return new SessionHandler(sessionsRepository);
};