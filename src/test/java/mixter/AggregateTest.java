package mixter;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

public abstract class AggregateTest {
    public List<Event> history(Event... events) {
        List<Event> eventHistory = new ArrayList<>();
        Collections.addAll(eventHistory, events);
        return eventHistory;
    }

    class SpyEventPublisher implements EventPublisher {
        public List<Event> publishedEvents = new ArrayList<>();

        public void publish(Event event) {
            publishedEvents.add(event);
        }
    }
}
