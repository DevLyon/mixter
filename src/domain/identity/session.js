var idGenerator = require('../../idGenerator');
var decisionProjection = require('../decisionProjection');

var SessionId = exports.SessionId = function SessionId(id){
    this.id = id;

    Object.freeze(this);
};

SessionId.prototype.toString = function toString(){
    return 'Session:' + this.id;
};

var UserConnected = exports.UserConnected = function UserConnected(sessionId, userIdentityId, connectedAt){
    this.sessionId = sessionId;
    this.userIdentityId = userIdentityId;
    this.connectedAt = connectedAt;

    Object.freeze(this);
};

var UserDisconnected = exports.UserDisconnected = function UserDisconnected(sessionId, userIdentityId){
    this.sessionId = sessionId;
    this.userIdentityId = userIdentityId;

    Object.freeze(this);
};

var Session = exports.Session = function Session(events){
    var projection = decisionProjection.create().register(UserConnected, function(event){
        this.userIdentityId = event.userIdentityId;
        this.sessionId = event.sessionId;
    }).apply(events);

    this.logOut = function logOut(publishEvent){
        publishEvent(new UserDisconnected(projection.sessionId, projection.userIdentityId));
    };
};

exports.logIn = function logIn(publishEvent, userIdentityId){
    var sessionId = new SessionId(idGenerator.generate());
    publishEvent(new UserConnected(sessionId, userIdentityId, new Date()));

    return sessionId;
};

exports.create = function create(events) {
    return new Session(events);
};