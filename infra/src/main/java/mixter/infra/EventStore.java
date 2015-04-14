package mixter.infra;

import mixter.domain.AggregateId;
import mixter.domain.Event;

import java.util.List;

public interface EventStore {
    List<Event> getEventsOfAggregate(AggregateId aggregateId);

    void store(Event event);
}
