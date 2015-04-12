var UserIdentityId = exports.UserIdentityId = function UserIdentity(email){
    this.email = email;

    Object.freeze(this);
};

UserIdentityId.prototype.toString = function(){
    return 'UserIdentity:' + this.email;
};

var UserRegistered = exports.UserRegistered = function UserRegistered(userIdentityId){
    this.userIdentityId = userIdentityId;

    Object.freeze(this);
};

exports.register = function register(publishEvent, email){
    var id = new UserIdentityId(email);
    publishEvent(new UserRegistered(id));
};