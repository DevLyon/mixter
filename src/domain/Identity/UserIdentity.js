var DecisionProjection = require('../DecisionProjection');
var Session = require('./Session');

var UserIdentityId = exports.UserIdentityId = function UserIdentity(email){
    this.email = email;

    Object.freeze(this);
};

UserIdentityId.prototype.toString = function toString(){
    return 'UserIdentity:' + this.email;
};

var UserRegistered = exports.UserRegistered = function UserRegistered(userIdentityId){
    this.userIdentityId = userIdentityId;

    Object.freeze(this);
};

var UserIdentity = function UserIdentity(events){
    var self = this;

    var projection = DecisionProjection.create().register(UserRegistered, function(event) {
        this.id = event.userIdentityId;
    }).apply(events);

    self.logIn = function logIn(publishEvent){
        Session.logIn(publishEvent, projection.id);
    };
};

exports.register = function register(publishEvent, email){
    var id = new UserIdentityId(email);
    publishEvent(new UserRegistered(id));
};

exports.create = function create(events){
    return new UserIdentity(events);
};