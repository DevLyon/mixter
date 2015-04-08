package mixter.domain.subscription;

import mixter.EventPublisher;
import mixter.UserId;
import mixter.domain.subscription.events.UserFollowed;

class Subscription {
    public static void follow(UserId follower, UserId followee, EventPublisher eventPublisher) {
        UserFollowed userFollowed = new UserFollowed(new SubscriptionId(follower, followee));
        eventPublisher.publish(userFollowed);
    }
}
