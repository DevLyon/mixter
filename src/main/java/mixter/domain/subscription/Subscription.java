package mixter.domain.subscription;

import mixter.EventPublisher;
import mixter.UserId;

class Subscription {
    public static void follow(UserId follower, UserId followee, EventPublisher eventPublisher) {
        UserFollowed userFollowed = new UserFollowed(new SubscriptionId(follower, followee));
        eventPublisher.publish(userFollowed);
    }
}
