package mixter.infra.repositories;

import mixter.domain.identity.Session;
import mixter.domain.identity.SessionId;
import mixter.infra.EventStore;

import java.util.NoSuchElementException;

public class EventSessionRepository {
    public EventSessionRepository(EventStore eventStore) {

    }

    public Session getById(SessionId sessionId) {
        throw new NoSuchElementException();
    }
}
