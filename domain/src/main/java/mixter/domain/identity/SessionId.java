package mixter.domain.identity;

import java.util.UUID;

public class SessionId {
    private String value;

    public SessionId(String value) {

        this.value = value;
    }

    public static SessionId generate() {
        return new SessionId(UUID.randomUUID().toString());
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;

        SessionId sessionId = (SessionId) o;

        return !(value != null ? !value.equals(sessionId.value) : sessionId.value != null);

    }

    @Override
    public int hashCode() {
        return value != null ? value.hashCode() : 0;
    }
}
