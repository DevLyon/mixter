var decisionProjection = require('../decisionProjection');
var UserId = require('../userId').UserId;
var session = require('./session');

var UserRegistered = exports.UserRegistered = function UserRegistered(userId){
    this.userId = userId;

    Object.freeze(this);
};

UserRegistered.prototype.getAggregateId = function getAggregateId(){
    return this.userId;
};

var UserIdentity = function UserIdentity(events){
    var self = this;

    var projection = decisionProjection.create().register(UserRegistered, function(event) {
        this.id = event.userId;
    }).apply(events);

    self.logIn = function logIn(publishEvent){
        return session.logIn(publishEvent, projection.id);
    };
};

exports.register = function register(publishEvent, email){
    var id = new UserId(email);
    publishEvent(new UserRegistered(id));
};

exports.create = function create(events){
    return new UserIdentity(events);
};