var Subscription = require('../../../src/domain/core/Subscription');
var UserId = require('../../../src/domain/UserId').UserId;
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
});