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

var FolloweeMessagePublished = exports.FolloweeMessagePublished = function FolloweeMessagePublished(subscriptionId, messageId){
    this.subscriptionId = subscriptionId;
    this.messageId = messageId;

    Object.freeze(this);
};

FolloweeMessagePublished.prototype.getAggregateId = function getAggregateId(){
    return this.subscriptionId;
};

exports.followUser = function followUser(publishEvent, follower, followee){
    publishEvent(new UserFollowed(new SubscriptionId(follower, followee)));
};