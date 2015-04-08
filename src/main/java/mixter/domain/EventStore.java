package mixter.domain;

import mixter.Event;
import mixter.domain.message.MessageId;

import java.util.List;

public interface EventStore {
    List<Event> getEventsForAggregate(MessageId messageId);
}
