package mixter.domain.timeline;

import mixter.UserId;
import mixter.domain.message.MessageId;
import mixter.domain.message.MessagePublished;
import org.junit.Test;

import java.util.ArrayList;
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
        UpdateTimeline handler = new UpdateTimeline(repository);
        // When
        handler.apply(messagePublished);
        // Then
        assertThat(repository.getMessages()).containsExactly(new TimelineMessage(author, author, content, messageId));
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
