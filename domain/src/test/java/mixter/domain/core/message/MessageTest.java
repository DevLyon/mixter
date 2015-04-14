package mixter.domain.core.message;

import mixter.domain.SpyEventPublisher;
import mixter.domain.UserId;
import mixter.domain.core.message.events.MessageQuacked;
import org.junit.Before;
import org.junit.Test;

import static org.assertj.core.api.Assertions.assertThat;

public class MessageTest {
    public String CONTENT = "content";
    public UserId AUTHOR_ID = new UserId();
    public SpyEventPublisher eventPublisher;

    @Before
    public void setUp() throws Exception {
        eventPublisher = new SpyEventPublisher();
    }

    @Test
    public void whenAMessageIsQuackedThenAMessageQuackedEventisPublished() {
        // Given

        // When
        MessageId messageId = Message.quack(AUTHOR_ID, CONTENT, eventPublisher);

        // Then
        MessageQuacked expectedEvent = new MessageQuacked(messageId, CONTENT, AUTHOR_ID);
        assertThat(eventPublisher.publishedEvents).extracting("message").containsExactly(expectedEvent.getMessage());
    }
}
