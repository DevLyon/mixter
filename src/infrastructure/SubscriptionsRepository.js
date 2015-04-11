var Subscription = require('../domain/core/Subscription');

var UnknownSubscription = exports.UnknownSubscription = function UnknownSubscription(subscriptionId){
    this.subscriptionId = subscriptionId;
};

var SubscriptionsRepository = function SubscriptionsRepository(eventsStore) {
    this.getSubscription = function getSubscription(subscriptionId){
        var events = eventsStore.getEventsOfAggregate(subscriptionId);
        if(!events.length){
            throw new UnknownSubscription(subscriptionId);
        }

        return Subscription.create(events);
    };
};

exports.create = function create(eventsStore){
    return new SubscriptionsRepository(eventsStore);
};