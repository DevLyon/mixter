package mixter.domain.timeline;

import mixter.Event;
import mixter.UserId;
import mixter.domain.EventStore;
import mixter.domain.message.MessageId;
import mixter.domain.message.MessagePublished;
import mixter.domain.message.MessageRepublished;
import org.junit.Test;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

import static org.assertj.core.api.Assertions.assertThat;

public class UpdateTimelineTest {
    @Test
    public void WhenUpdateTimelineAppliesAMessagePublishedEventThenATimelineMessageIsCreated() {
        // Given
        UserId author = new UserId();
        String content = "hello world";
        MessageId messageId = new MessageId();
        MessagePublished messagePublished = new MessagePublished(messageId, content, author);
        TimelineRepositoryFake repository = new TimelineRepositoryFake();
        EventStore eventStore = new FakeEventStore();
        UpdateTimeline handler = new UpdateTimeline(repository, eventStore);
        // When
        handler.apply(messagePublished);
        // Then
        assertThat(repository.getMessages()).containsExactly(new TimelineMessage(author, author, content, messageId));
    }

    @Test
    public void WhenUpdateTimelineAppliesAMessageRepublishedEventThenATimelineMessageIsCreated() {
        // Given
        UserId author = new UserId();
        UserId userId = new UserId();
        String content = "hello world";
        MessageId messageId = new MessageId();
        MessageRepublished messageRepublished = new MessageRepublished(messageId, userId);
        MessagePublished messagePublished = new MessagePublished(messageId, content, author);
        TimelineRepositoryFake repository = new TimelineRepositoryFake();
        EventStore eventStore = new FakeEventStore(messagePublished);
        UpdateTimeline handler = new UpdateTimeline(repository, eventStore);
        // When
        handler.apply(messageRepublished);
        // Then
        assertThat(repository.getMessages()).containsExactly(new TimelineMessage(userId, author, content, messageId));
    }

    class FakeEventStore implements EventStore {
        private List<Event> events;

        FakeEventStore(Event... events) {
            this(Arrays.asList(events));
        }

        FakeEventStore(List<Event> events) {
            this.events = events;
        }

        @Override
        public List<Event> getEventsForAggregate(MessageId messageId) {
            return events;
        }
    }

    class TimelineRepositoryFake implements TimelineRepository {
        List<TimelineMessage> messages = new ArrayList<>();

        public List<TimelineMessage> getMessages() {
            return messages;
        }

        @Override
        public TimelineMessage save(TimelineMessage message) {
            messages.removeIf(timelineMessage -> timelineMessage.getMessageId() == message.getMessageId());
            messages.add(message);
            return message;
        }
    }
}
