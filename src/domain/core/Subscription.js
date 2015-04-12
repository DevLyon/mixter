var valueType = require('../../valueType');

var SubscriptionId = exports.SubscriptionId = valueType.extends(function SubscriptionId(follower, followee){
    this.follower = follower;
    this.followee = followee;

    Object.freeze(this);
}, function toString() {
    return 'Subscription:' + this.follower + ' -> ' + this.followee;
});

var UserFollowed = exports.UserFollowed = function UserFollowed(subscriptionId){
    this.subscriptionId = subscriptionId;

    Object.freeze(this);
};

UserFollowed.prototype.getAggregateId = function getAggregateId(){
    return this.subscriptionId;
};

var UserUnfollowed = exports.UserUnfollowed = function UserUnfollowed(subscriptionId){
    this.subscriptionId = subscriptionId;

    Object.freeze(this);
};

UserUnfollowed.prototype.getAggregateId = function getAggregateId(){
    return this.subscriptionId;
};