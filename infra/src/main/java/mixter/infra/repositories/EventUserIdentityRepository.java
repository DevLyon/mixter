package mixter.infra.repositories;

import mixter.domain.Event;
import mixter.domain.identity.UserId;
import mixter.domain.identity.UserIdentity;
import mixter.infra.EventStore;

import java.util.List;
import java.util.NoSuchElementException;

public class EventUserIdentityRepository {
    private EventStore eventStore;

    public EventUserIdentityRepository(EventStore eventStore) {
        this.eventStore = eventStore;
    }

    public UserIdentity getById(UserId userId) {
        List<Event> history = eventStore.getEventsOfAggregate(userId);
        if (history.isEmpty()) {
            throw new NoSuchElementException();
        } else {
            return new UserIdentity(history);
        }
    }
}
