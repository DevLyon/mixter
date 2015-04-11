var EventsStore = require('../../src/infrastructure/EventsStore');
var SubscriptionsRepository = require('../../src/infrastructure/SubscriptionsRepository');
var Subscription = require('../../src/domain/core/Subscription');
var SubscriptionId = Subscription.SubscriptionId;
var UserId = require('../../src/domain/UserId').UserId;
var expect = require('chai').expect;

describe('Subscriptions Repository', function() {
    var subscriptionId = new SubscriptionId(new UserId('follower@mix-it.fr'), new UserId('followee@mix-it.fr'));
    var userFollowed = new Subscription.UserFollowed(subscriptionId);

    var repository;
    var eventsStore;
    beforeEach(function(){
        eventsStore = EventsStore.create();
        repository = SubscriptionsRepository.create(eventsStore);
    });

    it('Given UserFollowed When getSubscription Then return Subscription aggregate', function(done) {
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
        }).to.throw(SubscriptionsRepository.UnknownSubscription);
    });
});