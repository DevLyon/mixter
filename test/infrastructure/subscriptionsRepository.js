var createEventsStore = require('../../src/infrastructure/eventsStore').create;
var subscriptionsRepository = require('../../src/infrastructure/subscriptionsRepository');
var subscription = require('../../src/domain/core/subscription');
var SubscriptionId = subscription.SubscriptionId;
var UserId = require('../../src/domain/userId').UserId;
var expect = require('chai').expect;

describe('Subscriptions Repository', function() {
    var subscriptionId = new SubscriptionId(new UserId('follower@mix-it.fr'), new UserId('followee@mix-it.fr'));
    var userFollowed = new subscription.UserFollowed(subscriptionId);

    var repository;
    var eventsStore;
    beforeEach(function(){
        eventsStore = createEventsStore();
        repository = subscriptionsRepository.create(eventsStore);
    });

    it('Given UserFollowed When getSubscription Then return subscription aggregate', function(done) {
        eventsStore.store(userFollowed);

        var subscription = repository.getSubscription(subscriptionId);

        subscription.unfollow(function(event){
            expect(event.subscriptionId).to.eql(subscriptionId);

            done();
        });
    });

    it('Given no events When getSubscription Then throw UnknownSubscription', function() {
        expect(function () {
            repository.getSubscription(new SubscriptionId(new UserId('badfollower@mix-it.fr'), new UserId('badfollowee@mix-it.fr')));
        }).to.throw(subscriptionsRepository.UnknownSubscription);
    });
});