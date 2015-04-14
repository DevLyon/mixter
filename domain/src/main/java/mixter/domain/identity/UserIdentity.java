package mixter.domain.identity;

import mixter.domain.Event;
import mixter.domain.EventPublisher;
import mixter.domain.identity.events.UserConnected;
import mixter.domain.identity.events.UserRegistered;

import java.time.Instant;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
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

    private class DecisionProjection {
        public UserId userId;
        private Map<Class, Consumer> appliers = new HashMap<>();

        public DecisionProjection(List<Event> history) {
            Consumer<UserRegistered> applyUserRegistered = this::apply;
            appliers.put(UserRegistered.class, applyUserRegistered);
            history.forEach(this::apply);
        }

        @SuppressWarnings("unchecked")
        public void apply(Event event) {
            Consumer consumer = appliers.get(event.getClass());
            consumer.accept(event);
        }

        public void apply(UserRegistered event) {
            userId = event.getUserId();
        }

    }
}
