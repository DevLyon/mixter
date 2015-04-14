package mixter.infra;

import mixter.domain.Event;
import mixter.domain.EventPublisher;

import java.util.HashMap;
import java.util.HashSet;
import java.util.Map;
import java.util.Set;
import java.util.function.Consumer;

public class SynchronousEventPublisher implements EventPublisher {
    private Map<Class, Set<Consumer>> allHandlers = new HashMap<>();

    public <T> void register(Class<T> eventClass, Consumer<T> handler) {
        Set<Consumer> handlers = allHandlers.getOrDefault(eventClass, emptySet());
        handlers.add(handler);
        allHandlers.put(eventClass, handlers);
    }

    @SuppressWarnings("unchecked")
    public void publish(Event event) {
        Set<Consumer> handlers = allHandlers.getOrDefault(event.getClass(), emptySet());
        handlers.forEach(handler -> handler.accept(event));
    }

    private Set<Consumer> emptySet() {
        return new HashSet<>();
    }
}
