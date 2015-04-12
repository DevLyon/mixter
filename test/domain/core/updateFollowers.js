var followersRepository = require('../../../src/infrastructure/followersRepository');
var createEventPublisher = require('../../../src/infrastructure/eventPublisher').create;
var updateFollowers = require('../../../src/domain/core/updateFollowers');
var subscription = require('../../../src/domain/core/subscription');
var SubscriptionId = subscription.SubscriptionId;
var UserId = require('../../../src/domain/userId').UserId;
var expect = require('chai').expect;

describe('UpdateFollower Handler', function() {
    var subscriptionId = new SubscriptionId(new UserId('follower@mix-it.fr'), new UserId('followee@mix-it.fr'));

    var repository;
    var handler;
    var eventPublisher;
    beforeEach(function(){
        repository = followersRepository.create();
        handler = updateFollowers.create(repository);
        eventPublisher = createEventPublisher();
        handler.register(eventPublisher);
    });

    it('When UserFollowed Then save follower', function() {
        var userFollowed = new subscription.UserFollowed(subscriptionId);

        eventPublisher.publish(userFollowed);

        expect(repository.getFollowers(subscriptionId.followee)).to.contains(subscriptionId.follower);
    });

    it('When UserUnfollowed Then remove follower', function() {
        eventPublisher.publish(new subscription.UserFollowed(subscriptionId));

        eventPublisher.publish(new subscription.UserUnfollowed(subscriptionId));

        expect(repository.getFollowers(subscriptionId.followee)).to.be.empty;
    });
});