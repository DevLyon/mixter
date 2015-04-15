package mixter.infra.repositories;

import mixter.domain.AggregateId;
import mixter.domain.Event;
import mixter.infra.EventStore;

import java.util.List;
import java.util.NoSuchElementException;

public abstract class AggregateRepository<T> {
    protected EventStore eventStore;

    public AggregateRepository(EventStore eventStore) {
        this.eventStore = eventStore;
    }

    public T getById(AggregateId aggregateId) {
        List<Event> history = eventStore.getEventsOfAggregate(aggregateId);
        if (history.isEmpty()) {
            throw new NoSuchElementException();
        } else {
            return fromHistory(history);
        }
    }

    protected abstract T fromHistory(List<Event> history);
}
