var FollowersRepository = require('../../../src/infrastructure/FollowersRepository');
var EventPublisher = require('../../../src/infrastructure/EventPublisher');
var UpdateFollowers = require('../../../src/domain/core/UpdateFollowers');
var Subscription = require('../../../src/domain/core/Subscription');
var SubscriptionId = Subscription.SubscriptionId;
var UserId = require('../../../src/domain/UserId').UserId;
var expect = require('chai').expect;

describe('UpdateFollower Handler', function() {
    var subscriptionId = new SubscriptionId(new UserId('follower@mix-it.fr'), new UserId('followee@mix-it.fr'));

    var repository;
    var handler;
    var eventPublisher;
    beforeEach(function(){
        repository = FollowersRepository.create();
        handler = UpdateFollowers.create(repository);
        eventPublisher = EventPublisher.create();
        handler.register(eventPublisher);
    });

    it('When UserFollowed Then save follower', function() {
        var userFollowed = new Subscription.UserFollowed(subscriptionId);

        eventPublisher.publish(userFollowed);

        expect(repository.getFollowers(subscriptionId.followee)).to.contains(subscriptionId.follower);
    });

    it('When UserUnfollowed Then remove follower', function() {
        eventPublisher.publish(new Subscription.UserFollowed(subscriptionId));

        eventPublisher.publish(new Subscription.UserUnfollowed(subscriptionId));

        expect(repository.getFollowers(subscriptionId.followee)).to.be.empty;
    });
});