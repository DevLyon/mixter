package mixter.domain.core.subscription;

import mixter.doc.Aggregate;
import mixter.domain.EventPublisher;
import mixter.domain.core.subscription.events.UserFollowed;
import mixter.domain.identity.UserId;

@Aggregate
public class Subscription {
    public static void follow(UserId follower, UserId followee, EventPublisher eventPublisher) {
        eventPublisher.publish(new UserFollowed(new SubscriptionId(follower, followee)));
    }
}
