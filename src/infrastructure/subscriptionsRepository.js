var subscription = require('../domain/core/subscription');

var UnknownSubscription = exports.UnknownSubscription = function UnknownSubscription(subscriptionId){
    this.subscriptionId = subscriptionId;
};

var SubscriptionsRepository = function SubscriptionsRepository(eventsStore) {
    this.getSubscription = function getSubscription(subscriptionId){
        var events = eventsStore.getEventsOfAggregate(subscriptionId);
        if(!events.length){
            throw new UnknownSubscription(subscriptionId);
        }

        return subscription.create(events);
    };
};

exports.create = function create(eventsStore){
    return new SubscriptionsRepository(eventsStore);
};