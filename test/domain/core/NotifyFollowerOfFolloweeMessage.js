var SubscriptionsRepository = require('../../../src/infrastructure/SubscriptionsRepository');
var FollowersRepository = require('../../../src/infrastructure/FollowersRepository');
var EventsStore = require('../../../src/infrastructure/EventsStore');
var EventPublisher = require('../../../src/infrastructure/EventPublisher');
var NotifyFollowerOfFolloweeMessage = require('../../../src/domain/core/NotifyFollowerOfFolloweeMessage');
var UpdateFollowers = require('../../../src/domain/core/UpdateFollowers');
var Subscription = require('../../../src/domain/core/Subscription');
var SubscriptionId = Subscription.SubscriptionId;
var Message = require('../../../src/domain/core/Message');
var MessageId = Message.MessageId;
var UserId = require('../../../src/domain/UserId').UserId;
var expect = require('chai').expect;

describe('NotifyFollowerOfFolloweeMessage Handler', function() {
    var subscriptionId = new SubscriptionId(new UserId('follower@mix-it.fr'), new UserId('followee@mix-it.fr'));

    var eventPublisher;
    var events = [];
    beforeEach(function(){
        var eventsStore = EventsStore.create();
        var followersRepository = FollowersRepository.create();
        var subscriptionsRepository = SubscriptionsRepository.create(eventsStore, followersRepository);
        eventPublisher = EventPublisher.create();
        eventPublisher.onAny(function(event) {
            eventsStore.store(event);
            events.push(event);
        });
        UpdateFollowers.create(followersRepository).register(eventPublisher);
        NotifyFollowerOfFolloweeMessage.create(subscriptionsRepository).register(eventPublisher);
    });

    it('Given follower When MessagePublished by followee Then raise FolloweeMessagePublished', function() {
        eventPublisher.publish(new Subscription.UserFollowed(subscriptionId));
        var messagePublished = new Message.MessagePublished(new MessageId('M1'), subscriptionId.followee, 'Hello');

        eventPublisher.publish(messagePublished);

        var expectedEvent = new Subscription.FolloweeMessagePublished(subscriptionId, messagePublished.messageId);
        expect(events).to.contains(expectedEvent);
    });
});