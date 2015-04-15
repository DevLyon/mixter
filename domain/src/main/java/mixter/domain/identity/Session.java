package mixter.domain.identity;

import mixter.doc.Aggregate;
import mixter.doc.Projection;
import mixter.domain.DecisionProjectionBase;
import mixter.domain.Event;
import mixter.domain.EventPublisher;
import mixter.domain.identity.events.UserConnected;
import mixter.domain.identity.events.UserDisconnected;

import java.util.List;

@Aggregate
public class Session {

    private DecisionProjection projection;

    public Session(List<Event> history) {
        projection = new DecisionProjection(history);
    }

    public void logout(EventPublisher eventPublisher) {
        if (projection.active) {
            eventPublisher.publish(new UserDisconnected(projection.id, projection.userId));
        }
    }

    public SessionId getId() {
        return projection.id;
    }


    @Projection
    private class DecisionProjection extends DecisionProjectionBase {
        public SessionId id;
        public UserId userId;
        public boolean active = true;

        public DecisionProjection(List<Event> history) {
            register(UserConnected.class, this::apply);
            register(UserDisconnected.class, this::apply);
            history.forEach(this::apply);
        }

        public void apply(UserConnected event) {
            id = event.getSessionId();
            userId = event.getUserId();
        }

        public void apply(UserDisconnected event) {
            active = false;
        }
    }
}
