package mixter.domain.core.message.handlers;

import mixter.domain.core.message.MessageId;
import mixter.domain.core.message.TimelineMessageProjection;
import mixter.domain.core.message.TimelineMessageRepository;
import mixter.domain.core.message.events.MessageQuacked;
import mixter.domain.identity.UserId;
import org.junit.Before;
import org.junit.Test;

import java.util.ArrayList;
import java.util.List;

import static org.assertj.core.api.Assertions.assertThat;

public class UpdateTimelineTest {

    public static final UserId AUTHOR_ID = new UserId("author@mix-it.fr");
    public static final UserId USER_ID = new UserId("someUser@mix-it.fr");
    public static final String CONTENT = "hello world";
    private TimelineMessageRepositoryFake timelineRepository;

    @Before
    public void setUp() throws Exception {
        timelineRepository = new TimelineMessageRepositoryFake();
    }


    @Test
    public void whenUpdateTimelineAppliesAMessagePublishedEventThenATimelineMessageProjectionIsSaved() {
        // Given
        MessageId messageId = MessageId.generate();
        MessageQuacked messageQuacked = new MessageQuacked(messageId, CONTENT, AUTHOR_ID);
        UpdateTimeline handler = new UpdateTimeline(timelineRepository);
        // When
        handler.apply(messageQuacked);
        // Then
        assertThat(timelineRepository.getMessages()).containsExactly(new TimelineMessageProjection(AUTHOR_ID, AUTHOR_ID, CONTENT, messageId));
    }

    class TimelineMessageRepositoryFake implements TimelineMessageRepository {
        List<TimelineMessageProjection> messages = new ArrayList<>();

        public List<TimelineMessageProjection> getMessages() {
            return messages;
        }

        @Override
        public TimelineMessageProjection save(TimelineMessageProjection message) {
            messages.removeIf(timelineMessage -> timelineMessage.getMessageId().equals(message.getMessageId()));
            messages.add(message);
            return message;
        }

    }
}
