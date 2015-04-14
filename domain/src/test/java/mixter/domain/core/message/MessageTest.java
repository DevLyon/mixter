package mixter.domain.core.message;

import mixter.domain.SpyEventPublisher;
import mixter.domain.UserId;
import mixter.domain.core.message.events.MessagePublished;
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
    public void whenAMessageIsCreatedByAPublishMessageCommandThenItSendsAMessagePublishedEvent() {
        // Given
        PublishMessage publishMessage = new PublishMessage(CONTENT, AUTHOR_ID);

        // When
        MessageId messageId = Message.publish(publishMessage, eventPublisher);

        // Then
        MessagePublished expectedEvent = new MessagePublished(messageId, CONTENT, AUTHOR_ID);
        assertThat(eventPublisher.publishedEvents).extracting("message").containsExactly(expectedEvent.getMessage());
    }
}
