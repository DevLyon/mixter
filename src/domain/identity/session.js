var idGenerator = require('../../idGenerator');
var valueType = require('../../valueType');
var decisionProjection = require('../decisionProjection');

var SessionId = exports.SessionId = valueType.extends(function SessionId(id){
    this.id = id;
}, function toString() {
    return 'Session:' + this.id;
});

var UserConnected = exports.UserConnected = function UserConnected(sessionId, userId, connectedAt){
    this.sessionId = sessionId;
    this.userId = userId;
    this.connectedAt = connectedAt;

    Object.freeze(this);
};

UserConnected.prototype.getAggregateId = function getAggregateId(){
    return this.sessionId;
};

var UserDisconnected = exports.UserDisconnected = function UserDisconnected(sessionId, userId){
    this.sessionId = sessionId;
    this.userId = userId;

    Object.freeze(this);
};

UserDisconnected.prototype.getAggregateId = function getAggregateId(){
    return this.sessionId;
};

var Session = exports.Session = function Session(events){
    var projection = decisionProjection.create().register(UserConnected, function(event){
        this.userId = event.userId;
        this.sessionId = event.sessionId;
    }).register(UserDisconnected, function(event){
        this.isDisconnected = true;
    }).apply(events);

    this.logOut = function logOut(publishEvent){
        if(projection.isDisconnected){
            return;
        }

        publishEvent(new UserDisconnected(projection.sessionId, projection.userId));
    };
};

exports.logIn = function logIn(publishEvent, userId){
    var sessionId = new SessionId(idGenerator.generate());
    publishEvent(new UserConnected(sessionId, userId, new Date()));

    return sessionId;
};

exports.create = function create(events) {
    return new Session(events);
};