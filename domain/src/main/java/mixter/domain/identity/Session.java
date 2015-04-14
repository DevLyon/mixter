package mixter.domain.identity;

import mixter.domain.DecisionProjectionBase;
import mixter.domain.Event;
import mixter.domain.EventPublisher;
import mixter.domain.identity.events.UserConnected;
import mixter.domain.identity.events.UserDisconnected;

import java.util.List;
import java.util.function.Consumer;

public class Session {

    private DecisionProjection projection;

    public Session(List<Event> history) {
        projection = new DecisionProjection(history);
    }

    public void logout(EventPublisher eventPublisher) {
        eventPublisher.publish(new UserDisconnected(projection.id, projection.userId));
    }

    private class DecisionProjection extends DecisionProjectionBase {
        public SessionId id;
        public UserId userId;

        public DecisionProjection(List<Event> history) {
            Consumer<UserConnected> applyUserConnected = this::apply;
            register(UserConnected.class, applyUserConnected);
            history.forEach(this::apply);
        }

        public void apply(UserConnected event) {
            id = event.getSessionId();
            userId = event.getUserId();
        }

    }
}
