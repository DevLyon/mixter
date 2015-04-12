var Subscription = require('../../../src/domain/core/Subscription');
var UserId = require('../../../src/domain/UserId').UserId;
var MessageId = require('../../../src/domain/core/Message').MessageId;
var expect = require('chai').expect;

describe('Subscription Aggregate', function() {
    var follower = new UserId('follower@mix-it.fr');
    var followee = new UserId('followee@mix-it.fr');
    var subscriptionId = new Subscription.SubscriptionId(follower, followee);

    var eventsRaised = [];
    var publishEvent = function publishEvent(evt) {
        eventsRaised.push(evt);
    };

    beforeEach(function () {
        eventsRaised = [];
    });

    it('When create SubscriptionId Then toString return follower and followee', function() {
        expect(subscriptionId.toString()).to.equal('Subscription:' + follower + ' -> ' + followee);
    });

    it('When create UserFollowed Then aggregateId is subscriptionId', function() {
        var event = new Subscription.UserFollowed(subscriptionId);

        expect(event.getAggregateId()).to.equal(subscriptionId);
    });

    it('When create UserUnfollowed Then aggregateId is subscriptionId', function() {
        var event = new Subscription.UserUnfollowed(subscriptionId);

        expect(event.getAggregateId()).to.equal(subscriptionId);
    });

    it('When create FolloweeMessagePublished Then aggregateId is subscriptionId', function () {
        var event = new Subscription.FolloweeMessagePublished(subscriptionId, new MessageId('M1'));

        expect(event.getAggregateId()).to.equal(subscriptionId);
    });

    it('When Follow Then UserFollowed is raised', function () {
        Subscription.followUser(publishEvent, follower, followee);

        var expectedEvent = new Subscription.UserFollowed(new Subscription.SubscriptionId(follower, followee));
        expect(eventsRaised).to.contain(expectedEvent);
    });

    it('When unfollow Then UserUnfollowed is raised', function () {
        var subscription = Subscription.create(new Subscription.UserFollowed(subscriptionId));

        subscription.unfollow(publishEvent);

        var expectedEvent = new Subscription.UserUnfollowed(subscriptionId);
        expect(eventsRaised).to.contain(expectedEvent);
    });
});