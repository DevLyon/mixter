package mixter.domain.identity.events;

import mixter.domain.Event;
import mixter.domain.identity.SessionId;
import mixter.domain.identity.UserId;

public class UserDisconnected implements Event {
    private final SessionId sessionId;
    private final UserId userId;

    public UserDisconnected(SessionId sessionId, UserId userId) {
        this.sessionId = sessionId;
        this.userId = userId;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;

        UserDisconnected that = (UserDisconnected) o;

        if (sessionId != null ? !sessionId.equals(that.sessionId) : that.sessionId != null) return false;
        return !(userId != null ? !userId.equals(that.userId) : that.userId != null);

    }

    @Override
    public int hashCode() {
        int result = sessionId != null ? sessionId.hashCode() : 0;
        result = 31 * result + (userId != null ? userId.hashCode() : 0);
        return result;
    }
}
