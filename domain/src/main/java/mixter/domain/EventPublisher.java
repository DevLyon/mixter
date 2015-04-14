package mixter.domain;

import mixter.domain.core.message.events.MessageQuacked;

public interface EventPublisher {
    void publish(MessageQuacked messageQuacked);
}
