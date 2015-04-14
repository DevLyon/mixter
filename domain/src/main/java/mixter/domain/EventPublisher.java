package mixter.domain;

import mixter.domain.core.message.events.MessagePublished;

public interface EventPublisher {
    void publish(MessagePublished messagePublished);
}
