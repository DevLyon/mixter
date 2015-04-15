package mixter.domain.core.subscription.handlers;

import mixter.domain.EventPublisher;
import mixter.domain.core.message.MessageId;
import mixter.domain.core.message.events.MessagePublished;
import mixter.domain.core.message.events.MessageRepublished;
import mixter.domain.core.message.events.ReplyMessagePublished;
import mixter.domain.core.subscription.FollowerRepository;
import mixter.domain.core.subscription.Subscription;
import mixter.domain.core.subscription.SubscriptionId;
import mixter.domain.core.subscription.SubscriptionRepository;
import mixter.domain.identity.UserId;

import java.util.Set;

public class NotifyFollowerOfFolloweeMessage {

    private final FollowerRepository followerRepository;
    private final SubscriptionRepository subscriptionRepository;
    private final EventPublisher eventPublisher;

    public NotifyFollowerOfFolloweeMessage(FollowerRepository followerRepository, SubscriptionRepository subscriptionRepository, EventPublisher eventPublisher) {

        this.followerRepository = followerRepository;
        this.subscriptionRepository = subscriptionRepository;
        this.eventPublisher = eventPublisher;
    }

    public void apply(MessagePublished messagePublished) {
        notifyFollowers(messagePublished.getAuthorId(), messagePublished.getMessageId());
    }

    private void notifyFollowers(UserId followeee, MessageId messageId) {
        Set<UserId> followers = followerRepository.getFollowers(followeee);
        for (UserId follower : followers) {
            Subscription subscription = subscriptionRepository.getById(new SubscriptionId(follower, followeee));
            subscription.notifyFollower(messageId, eventPublisher);
        }
    }

    public void apply(MessageRepublished event) {
        notifyFollowers(event.getAuthorId(), event.getMessageId());
    }

    public void apply(ReplyMessagePublished messageReplied) {
        notifyFollowers(messageReplied.getAuthorId(), messageReplied.getMessageId());
    }
}
