var valueType = require('../../valueType');
var decisionProjection = require('../decisionProjection');

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

var FolloweeMessageQuacked = exports.FolloweeMessageQuacked = function FolloweeMessageQuacked(subscriptionId, messageId){
    this.subscriptionId = subscriptionId;
    this.messageId = messageId;

    Object.freeze(this);
};

FolloweeMessageQuacked.prototype.getAggregateId = function getAggregateId(){
    return this.subscriptionId;
};

var Subscription = exports.Subscription = function Subscription(events){
    var self = this;

    var projection = decisionProjection.create().register(UserFollowed, function(event) {
        this.subscriptionId = event.subscriptionId;
    }).apply(events);

    self.unfollow = function(publishEvent) {
        publishEvent(new UserUnfollowed(projection.subscriptionId));
    };
};

exports.followUser = function followUser(publishEvent, follower, followee){
    publishEvent(new UserFollowed(new SubscriptionId(follower, followee)));
};

exports.create = function create(events){
    return new Subscription(events);
};