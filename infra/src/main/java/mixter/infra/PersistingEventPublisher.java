package mixter.infra;

import mixter.domain.Event;
import mixter.domain.EventPublisher;

public class PersistingEventPublisher implements EventPublisher {
    private final EventStore store;
    private final EventPublisher publisher;

    public PersistingEventPublisher(EventStore store, EventPublisher publisher) {
        this.store = store;
        this.publisher = publisher;
    }

    @Override
    public void publish(Event event) {
        store.store(event);
        publisher.publish(event);
    }
}
