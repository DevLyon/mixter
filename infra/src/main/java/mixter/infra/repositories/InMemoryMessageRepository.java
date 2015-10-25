package mixter.infra.repositories;

import mixter.domain.Event;
import mixter.domain.core.message.Message;
import mixter.domain.core.message.MessageId;
import mixter.domain.core.message.MessageRepository;
import mixter.infra.EventStore;

import java.util.List;

public class InMemoryMessageRepository extends AggregateRepository<Message> implements MessageRepository{

    public InMemoryMessageRepository(EventStore eventStore) {
        super(eventStore);
    }

    @Override
    protected Message fromHistory(List<Event> history) {
        return new Message(history);
    }

    @Override
    public Message getById(MessageId messageId) {
        return super.getById(messageId);
    }
}
