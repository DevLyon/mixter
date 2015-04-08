package mixter.domain.subscription;

import mixter.Event;
import mixter.EventPublisher;
import mixter.UserId;
import mixter.domain.subscription.events.UserFollowed;
import mixter.domain.subscription.events.UserUnfollowed;

import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.function.Consumer;

class Subscription {
    private final DecisionProjection projection;

    public Subscription(List<Event> events) {
        projection = new DecisionProjection(events);
    }

    public static void follow(UserId follower, UserId followee, EventPublisher eventPublisher) {
        UserFollowed userFollowed = new UserFollowed(new SubscriptionId(follower, followee));
        eventPublisher.publish(userFollowed);
    }

    public void unfollow(EventPublisher eventPublisher) {
        eventPublisher.publish(new UserUnfollowed(new SubscriptionId(projection.follower, projection.followee)));
    }

    class DecisionProjection {
        public UserId follower;
        private Map<Class, Consumer> appliers = new HashMap<>();
        public UserId followee;

        public DecisionProjection(List<Event> eventHistory) {
            Consumer<UserFollowed> applyMessageDeleted = this::apply;
            appliers.put(UserFollowed.class, applyMessageDeleted);
            eventHistory.forEach(this::apply);
        }

        @SuppressWarnings("unchecked")
        public void apply(Event event) {
            Consumer consumer = appliers.get(event.getClass());
            consumer.accept(event);
        }

        private void apply(UserFollowed userFollowed) {
            follower = userFollowed.getSubscriptionId().getFollower();
            followee = userFollowed.getSubscriptionId().getFollowee();
        }
    }
}
