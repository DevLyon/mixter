package mixter.infra;

import mixter.domain.Event;
import mixter.domain.EventPublisher;

import java.util.HashSet;
import java.util.Set;
import java.util.function.Consumer;

public class SynchronousEventPublisher implements EventPublisher {
    private Set<Consumer> handlers = new HashSet<>();

    public <T> void register(Class<T> eventClass, Consumer<T> handler) {
        this.handlers.add(handler);
    }

    @SuppressWarnings("unchecked")
    public void publish(Event event) {
        handlers.forEach(handler -> handler.accept(event));
    }
}
