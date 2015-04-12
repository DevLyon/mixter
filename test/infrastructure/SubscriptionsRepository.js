var EventsStore = require('../../src/infrastructure/EventsStore');
var SubscriptionsRepository = require('../../src/infrastructure/SubscriptionsRepository');
var Subscription = require('../../src/domain/core/Subscription');
var FollowersRepository = require('../../src/infrastructure/FollowersRepository');
var FollowerProjection = require('../../src/domain/core/FollowerProjection');
var SubscriptionId = Subscription.SubscriptionId;
var UserId = require('../../src/domain/UserId').UserId;
var expect = require('chai').expect;

describe('Subscriptions Repository', function() {
    var subscriptionId = new SubscriptionId(new UserId('follower@mix-it.fr'), new UserId('followee@mix-it.fr'));
    var userFollowed = new Subscription.UserFollowed(subscriptionId);

    var subscriptionsRepository;
    var followersRepository;
    var eventsStore;
    beforeEach(function(){
        eventsStore = EventsStore.create();
        followersRepository = FollowersRepository.create();
        subscriptionsRepository = SubscriptionsRepository.create(eventsStore, followersRepository);
    });

    it('Given UserFollowed When getSubscription Then return Subscription aggregate', function(done) {
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
        }).to.throw(SubscriptionsRepository.UnknownSubscription);
    });

    it('When getSubscriptionsOfUser Then return all Subscription aggregates of user', function() {
        var followee = new UserId('followee@mix-it.fr');
        var follower1 = new UserId('follower1@mix-it.fr');
        var follower2 = new UserId('follower2@mix-it.fr');
        followersRepository.save(FollowerProjection.create(followee, follower1));
        followersRepository.save(FollowerProjection.create(followee, follower2));
        eventsStore.store(new Subscription.UserFollowed(new Subscription.SubscriptionId(follower1, followee)));
        eventsStore.store(new Subscription.UserFollowed(new Subscription.SubscriptionId(follower2, followee)));

        var subscriptions = subscriptionsRepository.getSubscriptionsOfUser(followee);

        expect(subscriptions).to.have.length(2);
    });
});