package mixter.infra.repositories;

import mixter.domain.Event;
import mixter.domain.identity.Session;
import mixter.infra.EventStore;

import java.util.List;

public class EventSessionRepository extends AggregateRepository<Session> {

    public EventSessionRepository(EventStore eventStore) {
        super(eventStore);
    }

    @Override
    protected Session fromHistory(List<Event> history) {
        return new Session(history);
    }


}
