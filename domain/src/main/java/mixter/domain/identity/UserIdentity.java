package mixter.domain.identity;

import mixter.domain.DecisionProjectionBase;
import mixter.domain.Event;
import mixter.domain.EventPublisher;
import mixter.domain.identity.events.UserConnected;
import mixter.domain.identity.events.UserRegistered;

import java.time.Instant;
import java.util.List;
import java.util.function.Consumer;

public class UserIdentity {
    private DecisionProjection projection;

    public UserIdentity(List<Event> history) {
        projection = new DecisionProjection(history);
    }

    public static void register(EventPublisher eventPublisher, UserId userId) {
        eventPublisher.publish(new UserRegistered(userId));
    }

    public SessionId logIn(EventPublisher eventPublisher) {
        SessionId sessionId = new SessionId();
        eventPublisher.publish(new UserConnected(sessionId, projection.userId, Instant.now()));
        return sessionId;
    }

    private class DecisionProjection extends DecisionProjectionBase {
        public UserId userId;

        public DecisionProjection(List<Event> history) {
            Consumer<UserRegistered> applyUserRegistered = this::apply;
            register(UserRegistered.class, applyUserRegistered);
            history.forEach(this::apply);
        }

        public void apply(UserRegistered event) {
            userId = event.getUserId();
        }
    }
}
