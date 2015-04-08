package mixter.domain.message.handlers;

import mixter.Event;
import mixter.domain.EventStore;
import mixter.domain.message.TimelineMessage;
import mixter.domain.message.TimelineRepository;
import mixter.domain.message.events.MessagePublished;
import mixter.domain.message.events.MessageRepublished;

public class UpdateTimeline {

    private final TimelineRepository repository;
    private EventStore eventStore;

    public UpdateTimeline(TimelineRepository repository, EventStore eventStore) {
        this.repository = repository;
        this.eventStore = eventStore;
    }

    public void apply(MessagePublished message) {
        repository.save(new TimelineMessage(message.getAuthorId(), message.getAuthorId(), message.getMessage(), message.getMessageId()));
    }

    public void apply(MessageRepublished message) {
        Event event = eventStore.getEventsForAggregate(message.getMessageId()).get(0);
        assert (event instanceof MessagePublished); // the creating event of a message is always a MessagePublished for now
        MessagePublished creatingEvent = (MessagePublished) event;
        repository.save(new TimelineMessage(message.getUserId(), creatingEvent.getAuthorId(), creatingEvent.getMessage(), creatingEvent.getMessageId()));
    }
}
