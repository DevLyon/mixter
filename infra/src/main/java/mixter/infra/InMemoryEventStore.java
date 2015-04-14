package mixter.infra;

import mixter.domain.AggregateId;
import mixter.domain.Event;

import java.util.ArrayList;
import java.util.List;

public class InMemoryEventStore implements EventStore {
    List<Event> events = new ArrayList<>();

    @Override
    public List<Event> getEventsOfAggregate(AggregateId aggregateId) {
        return events;
    }

    @Override
    public void store(Event event) {
        events.add(event);
    }
}
