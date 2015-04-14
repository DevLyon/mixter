package mixter.infra;

import mixter.domain.AggregateId;
import mixter.domain.Event;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class InMemoryEventStore implements EventStore {
    private Map<AggregateId, List<Event>> events = new HashMap<>();

    private List<Event> emptyList() {
        return new ArrayList<>();
    }

    @Override
    public List<Event> getEventsOfAggregate(AggregateId aggregateId) {
        return events.getOrDefault(aggregateId, emptyList());
    }

    @Override
    public void store(Event event) {
        List<Event> aggregateEvents = events.getOrDefault(event.getId(), emptyList());
        aggregateEvents.add(event);
        events.put(event.getId(), aggregateEvents);
    }
}
