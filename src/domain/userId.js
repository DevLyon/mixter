var valueType = require('../valueType');

var UserEmailCannotBeEmpty = exports.UserEmailCannotBeEmpty = function UserEmailCannotBeEmpty(){};

var UserId = exports.UserId = valueType.extends(function UserId(email){
    if(!email){
        throw new UserEmailCannotBeEmpty();
    }

    this.email = email;
}, function toString(){
    return 'UserId:' + this.email;
});