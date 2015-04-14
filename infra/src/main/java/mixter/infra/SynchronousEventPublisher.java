package mixter.infra;

import mixter.domain.Event;
import mixter.domain.EventPublisher;

import java.util.function.Consumer;

public class SynchronousEventPublisher implements EventPublisher {
    private Consumer handler;

    public <T> void register(Class<T> eventClass, Consumer<T> handler) {
        this.handler = handler;
    }

    @SuppressWarnings("unchecked")
    public void publish(Event event) {
        handler.accept(event);
    }
}
