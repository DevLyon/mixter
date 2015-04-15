package mixter.domain.core.message;

import mixter.domain.Event;
import mixter.domain.SpyEventPublisher;
import mixter.domain.identity.UserId;
import mixter.domain.core.message.events.MessageQuacked;
import mixter.domain.core.message.events.MessageRequacked;
import mixter.domain.core.message.events.MessageDeleted;
import org.junit.Before;
import org.junit.Test;

import java.util.List;

import static org.assertj.core.api.Assertions.assertThat;

public class MessageTest extends mixter.domain.DomainTest {
    public String CONTENT = "content";
    public UserId AUTHOR_ID = new UserId("author@mix-it.fr");
    public UserId USER_ID = new UserId("user@mix-it.fr");
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

    @Test
    public void whenAMessageIsRequackedThenItSendsAMessageRequackedEvent() {
        // Given
        MessageId messageId = MessageId.generate();
        Message message = messageFor(
                new MessageQuacked(messageId, CONTENT, AUTHOR_ID)
        );

        // When
        message.reQuack(USER_ID, eventPublisher, AUTHOR_ID, CONTENT);

        // Then
        MessageRequacked expectedEvent = new MessageRequacked(messageId, USER_ID, AUTHOR_ID, CONTENT);
        assertThat(eventPublisher.publishedEvents).containsExactly(expectedEvent);
    }

    @Test
    public void whenAMessageIsRequackedByItsAuthorThenItShouldNotSendRequackedEvent() {
        // Given
        MessageId messageId = MessageId.generate();

        Message message = messageFor(
                new MessageQuacked(messageId, CONTENT, AUTHOR_ID)
        );

        // When
        message.reQuack(AUTHOR_ID, eventPublisher, AUTHOR_ID, CONTENT);

        // Then
        assertThat(eventPublisher.publishedEvents).isEmpty();
    }

    @Test
    public void whenAMessageIsRequackedTwiceByTheSameUserThenItShouldNotSendMessageRequackedEvent() {
        // Given
        MessageId messageId = MessageId.generate();
        Message message = messageFor(
                new MessageQuacked(messageId, CONTENT, AUTHOR_ID),
                new MessageRequacked(messageId, USER_ID, AUTHOR_ID, CONTENT)
        );

        // When
        message.reQuack(USER_ID, eventPublisher, AUTHOR_ID, CONTENT);

        // Then
        assertThat(eventPublisher.publishedEvents).isEmpty();
    }

    @Test
    public void whenAMessageIsDeletedByItsAuthorThenItShouldSendMessageDeletedEvent() {
        // Given
        MessageId messageId = MessageId.generate();
        List<Event> eventHistory = history(
                new MessageQuacked(messageId, CONTENT, AUTHOR_ID)
        );

        Message message = new Message(eventHistory);

        // When
        message.delete(AUTHOR_ID, eventPublisher);

        // Then
        MessageDeleted expectedEvent = new MessageDeleted(messageId);
        assertThat(eventPublisher.publishedEvents).containsExactly(expectedEvent);
    }

    protected Message messageFor(Event... events) {
        return new Message(history(events));
    }
}
