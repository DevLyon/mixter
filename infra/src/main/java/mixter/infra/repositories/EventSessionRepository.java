package mixter.infra.repositories;

import mixter.domain.Event;
import mixter.domain.identity.Session;
import mixter.domain.identity.SessionId;
import mixter.infra.EventStore;

import java.util.List;
import java.util.NoSuchElementException;

public class EventSessionRepository {
    private EventStore eventStore;

    public EventSessionRepository(EventStore eventStore) {
        this.eventStore = eventStore;
    }

    public Session getById(SessionId sessionId) {
        List<Event> history = eventStore.getEventsOfAggregate(sessionId);
        if (history.isEmpty()) {
            throw new NoSuchElementException();
        } else {
            return new Session(history);
        }
    }
}
