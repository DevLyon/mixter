var subscription = require('../../../src/domain/core/subscription');
var UserId = require('../../../src/domain/userId').UserId;
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
});