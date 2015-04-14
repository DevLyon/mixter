package mixter.domain.identity;

public class SessionProjection {
    private final SessionId sessionId;
    private final UserId userId;
    private final SessionStatus status;

    public SessionProjection(SessionId sessionId, UserId userId, SessionStatus status) {
        this.sessionId = sessionId;
        this.userId = userId;
        this.status = status;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;

        SessionProjection that = (SessionProjection) o;

        if (sessionId != null ? !sessionId.equals(that.sessionId) : that.sessionId != null) return false;
        if (userId != null ? !userId.equals(that.userId) : that.userId != null) return false;
        return status == that.status;

    }

    @Override
    public int hashCode() {
        int result = sessionId != null ? sessionId.hashCode() : 0;
        result = 31 * result + (userId != null ? userId.hashCode() : 0);
        result = 31 * result + (status != null ? status.hashCode() : 0);
        return result;
    }
}
