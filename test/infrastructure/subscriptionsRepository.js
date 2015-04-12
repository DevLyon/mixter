var createEventsStore = require('../../src/infrastructure/eventsStore').create;
var createSubscriptionsRepository = require('../../src/infrastructure/subscriptionsRepository').create;
var subscription = require('../../src/domain/core/subscription');
var createFollowersRepository = require('../../src/infrastructure/followersRepository').create;
var followerProjection = require('../../src/domain/core/followerProjection');
var SubscriptionId = subscription.SubscriptionId;
var UserId = require('../../src/domain/userId').UserId;
var expect = require('chai').expect;

describe('Subscriptions Repository', function() {
    var subscriptionId = new SubscriptionId(new UserId('follower@mix-it.fr'), new UserId('followee@mix-it.fr'));
    var userFollowed = new subscription.UserFollowed(subscriptionId);

    var subscriptionsRepository;
    var followersRepository;
    var eventsStore;
    beforeEach(function(){
        eventsStore = createEventsStore();
        followersRepository = createFollowersRepository();
        subscriptionsRepository = createSubscriptionsRepository(eventsStore, followersRepository);
    });

    it('Given UserFollowed When getSubscription Then return subscription aggregate', function(done) {
        eventsStore.store(userFollowed);

        var subscription = subscriptionsRepository.getSubscription(subscriptionId);

        subscription.unfollow(function(event){
            expect(event.subscriptionId).to.eql(subscriptionId);

            done();
        });
    });

    it('Given no events When getSubscription Then throw UnknownSubscription', function() {
        expect(function () {
            subscriptionsRepository.getSubscription(new SubscriptionId(new UserId('badfollower@mix-it.fr'), new UserId('badfollowee@mix-it.fr')));
        }).to.throw(subscriptionsRepository.UnknownSubscription);
    });

    it('When getSubscriptionsOfUser Then return all Subscription aggregates of user', function() {
        var followee = new UserId('followee@mix-it.fr');
        var follower1 = new UserId('follower1@mix-it.fr');
        var follower2 = new UserId('follower2@mix-it.fr');
        followersRepository.save(followerProjection.create(followee, follower1));
        followersRepository.save(followerProjection.create(followee, follower2));
        eventsStore.store(new subscription.UserFollowed(new SubscriptionId(follower1, followee)));
        eventsStore.store(new subscription.UserFollowed(new SubscriptionId(follower2, followee)));

        var subscriptions = subscriptionsRepository.getSubscriptionsOfUser(followee);

        expect(subscriptions).to.have.length(2);
    });
});