var subscription = require('../../../src/domain/core/subscription');
var UserId = require('../../../src/domain/userId').UserId;
var MessageId = require('../../../src/domain/core/message').MessageId;
var expect = require('chai').expect;

describe('Subscription Aggregate', function() {
    var follower = new UserId('follower@mix-it.fr');
    var followee = new UserId('followee@mix-it.fr');
    var subscriptionId = new subscription.SubscriptionId(follower, followee);

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
        var event = new subscription.UserFollowed(subscriptionId);

        expect(event.getAggregateId()).to.equal(subscriptionId);
    });

    it('When create UserUnfollowed Then aggregateId is subscriptionId', function() {
        var event = new subscription.UserUnfollowed(subscriptionId);

        expect(event.getAggregateId()).to.equal(subscriptionId);
    });

    it('When create FolloweeMessageQuacked Then aggregateId is subscriptionId', function () {
        var event = new subscription.FolloweeMessageQuacked(subscriptionId, new MessageId('M1'));

        expect(event.getAggregateId()).to.equal(subscriptionId);
    });

    it('When Follow Then UserFollowed is raised', function () {
        subscription.followUser(publishEvent, follower, followee);

        var expectedEvent = new subscription.UserFollowed(new subscription.SubscriptionId(follower, followee));
        expect(eventsRaised).to.contain(expectedEvent);
    });
});