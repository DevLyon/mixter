package mixter.domain.core.subscription;

import mixter.doc.Aggregate;
import mixter.domain.DecisionProjectionBase;
import mixter.domain.Event;
import mixter.domain.EventPublisher;
import mixter.domain.core.message.MessageId;
import mixter.domain.core.subscription.events.FolloweeMessageQuacked;
import mixter.domain.core.subscription.events.UserFollowed;
import mixter.domain.core.subscription.events.UserUnfollowed;
import mixter.domain.identity.UserId;

import java.util.List;

@Aggregate
public class Subscription {
    private DecisionProjection projection;

    public Subscription(List<Event> history) {
        projection = new DecisionProjection(history);
    }

    public static void follow(UserId follower, UserId followee, EventPublisher eventPublisher) {
        eventPublisher.publish(new UserFollowed(new SubscriptionId(follower, followee)));
    }

    public void unfollow(EventPublisher eventPublisher) {
        eventPublisher.publish(new UserUnfollowed(projection.id));
    }

    public void notifyFollower(MessageId messageId, EventPublisher eventPublisher) {
        if (projection.active) {
            eventPublisher.publish(new FolloweeMessageQuacked(projection.id, messageId));
        }
    }

    public SubscriptionId getId() {
        return projection.id;
    }

    private class DecisionProjection extends DecisionProjectionBase {
        public SubscriptionId id;
        public boolean active = true;

        public DecisionProjection(List<Event> history) {
            super();
            super.register(UserFollowed.class, this::apply);
            super.register(UserUnfollowed.class, this::apply);
            history.forEach(this::apply);
        }

        private void apply(UserFollowed event) {
            id = event.getSubscriptionId();
        }

        private void apply(UserUnfollowed event) {
            active = false;
        }
    }
}
