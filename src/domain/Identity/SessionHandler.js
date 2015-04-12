var SessionProjection = require('./SessionProjection');
var Session = require('./Session');

var SessionHandler = function SessionHandler(sessionsRepository){
    var saveProjection = function saveProjection(event, isEnabled){
        var projection = new SessionProjection.create(event.sessionId, event.userId, isEnabled);
        sessionsRepository.save(projection);
    };

    this.register = function register(eventPublisher) {
        eventPublisher.on(Session.UserConnected, function(event){
            saveProjection(event, SessionProjection.SessionEnabled);
        }).on(Session.UserDisconnected, function(event){
            saveProjection(event, SessionProjection.SessionDisabled);
        });
    };
};

exports.create = function(sessionsRepository){
    return new SessionHandler(sessionsRepository);
};