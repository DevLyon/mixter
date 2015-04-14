package mixter.domain.identity.events;

import mixter.domain.AggregateId;
import mixter.domain.Event;
import mixter.domain.identity.SessionId;
import mixter.domain.identity.UserId;

import java.time.Instant;

public class UserConnected implements Event {
    private final SessionId sessionId;
    private final UserId userId;
    private final Instant now;

    public UserConnected(SessionId sessionId, UserId userId, Instant now) {
        this.sessionId = sessionId;
        this.userId = userId;
        this.now = now;
    }

    public SessionId getSessionId() {
        return sessionId;
    }

    public UserId getUserId() {
        return userId;
    }

    public Instant getNow() {
        return now;
    }

    @Override
    public AggregateId getId() {
        return userId;
    }
}
