package mixter.domain;

import java.util.HashMap;
import java.util.Map;
import java.util.function.Consumer;

public abstract class DecisionProjectionBase {
    private Map<Class, Consumer> appliers = new HashMap<>();

    public <T> void register(Class<T> eventClass, Consumer<T> eventConsumer) {
        appliers.put(eventClass, eventConsumer);
    }

    @SuppressWarnings("unchecked")
    public void apply(Event event) {
        Consumer consumer = appliers.get(event.getClass());
        consumer.accept(event);
    }
}
