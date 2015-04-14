package mixter.domain.identity.events;

import mixter.domain.AggregateId;
import mixter.domain.Event;
import mixter.domain.identity.UserId;

public class UserRegistered implements Event {
    private UserId userId;

    public UserRegistered(UserId userId) {
        this.userId = userId;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;

        UserRegistered that = (UserRegistered) o;

        return !(userId != null ? !userId.equals(that.userId) : that.userId != null);

    }

    @Override
    public int hashCode() {
        return userId != null ? userId.hashCode() : 0;
    }

    public UserId getUserId() {
        return userId;
    }

    @Override
    public AggregateId getId() {
        return userId;
    }
}
