package mixter.domain.identity;

import mixter.domain.EventPublisher;
import mixter.domain.identity.events.UserRegistered;

public class UserIdentity {
    public static void register(EventPublisher eventPublisher, UserId userId) {
        eventPublisher.publish(new UserRegistered(userId));
    }
}
