package mixter.domain.core.message;

import mixter.domain.Event;
import mixter.domain.SpyEventPublisher;
import mixter.domain.UserId;
import mixter.domain.core.message.events.MessageQuacked;
import mixter.domain.core.message.events.MessageRequacked;
import org.junit.Before;
import org.junit.Test;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

import static org.assertj.core.api.Assertions.assertThat;

public class MessageTest {
    public String CONTENT = "content";
    public UserId AUTHOR_ID = new UserId();
    public UserId USER_ID = new UserId();
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
        MessageId messageId = new MessageId();
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
        MessageId messageId = new MessageId();

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
        MessageId messageId = new MessageId();
        Message message = messageFor(
                new MessageQuacked(messageId, CONTENT, AUTHOR_ID),
                new MessageRequacked(messageId, USER_ID, AUTHOR_ID, CONTENT)
        );

        // When
        message.reQuack(USER_ID, eventPublisher, AUTHOR_ID, CONTENT);

        // Then
        assertThat(eventPublisher.publishedEvents).isEmpty();
    }



    protected Message messageFor(Event... events) {
        return new Message(history(events));
    }
    protected List<Event> history(Event... events) {
        List<Event> eventHistory = new ArrayList<>();
        Collections.addAll(eventHistory, events);
        return eventHistory;
    }
}
