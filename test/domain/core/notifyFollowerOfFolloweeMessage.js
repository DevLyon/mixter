var createSubscriptionsRepository = require('../../../src/infrastructure/subscriptionsRepository').create;
var createFollowersRepository = require('../../../src/infrastructure/followersRepository').create;
var createEventsStore = require('../../../src/infrastructure/eventsStore').create;
var createEventPublisher = require('../../../src/infrastructure/eventPublisher').create;
var notifyFollowerOfFolloweeMessage = require('../../../src/domain/core/notifyFollowerOfFolloweeMessage');
var updateFollowers = require('../../../src/domain/core/updateFollowers');
var subscription = require('../../../src/domain/core/subscription');
var SubscriptionId = subscription.SubscriptionId;
var message = require('../../../src/domain/core/message');
var MessageId = message.MessageId;
var UserId = require('../../../src/domain/userId').UserId;
var expect = require('chai').expect;

describe('NotifyFollowerOfFolloweeMessage Handler', function() {
    var subscriptionId = new SubscriptionId(new UserId('follower@mix-it.fr'), new UserId('followee@mix-it.fr'));

    var eventPublisher;
    var events = [];
    beforeEach(function(){
        var eventsStore = createEventsStore();
        var followersRepository = createFollowersRepository();
        var subscriptionsRepository = createSubscriptionsRepository(eventsStore, followersRepository);
        eventPublisher = createEventPublisher();
        eventPublisher.onAny(function(event) {
            eventsStore.store(event);
            events.push(event);
        });
        updateFollowers.create(followersRepository).register(eventPublisher);
        notifyFollowerOfFolloweeMessage.create(subscriptionsRepository).register(eventPublisher);
    });

    it('Given follower When MessageQuacked by followee Then raise FolloweeMessageQuacked', function() {
        eventPublisher.publish(new subscription.UserFollowed(subscriptionId));
        var messageQuacked = new message.MessageQuacked(new MessageId('M1'), subscriptionId.followee, 'Hello');

        eventPublisher.publish(messageQuacked);

        var expectedEvent = new subscription.FolloweeMessageQuacked(subscriptionId, messageQuacked.messageId);
        expect(events).to.contains(expectedEvent);
    });
});