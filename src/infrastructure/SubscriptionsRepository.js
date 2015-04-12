var Subscription = require('../domain/core/Subscription');
var _ = require('lodash');

var UnknownSubscription = exports.UnknownSubscription = function UnknownSubscription(subscriptionId){
    this.subscriptionId = subscriptionId;
};

var SubscriptionsRepository = function SubscriptionsRepository(eventsStore, followersRepository) {
    var self = this;

    self.getSubscription = function getSubscription(subscriptionId){
        var events = eventsStore.getEventsOfAggregate(subscriptionId);
        if(!events.length){
            throw new UnknownSubscription(subscriptionId);
        }

        return Subscription.create(events);
    };

    self.getSubscriptionsOfUser = function getSubscriptionsOfUser(userId){
        return _(followersRepository.getFollowers(userId)).map(function(follower) {
            return new Subscription.SubscriptionId(follower, userId);
        }).map(function(subscriptionId){
            return self.getSubscription(subscriptionId);
        }).value();
    };
};

exports.create = function create(eventsStore, followersRepository){
    return new SubscriptionsRepository(eventsStore, followersRepository);
};