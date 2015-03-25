package mixter;

import org.junit.Test;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

import static org.assertj.core.api.Assertions.assertThat;

public class MessageTest {
    @Test
    public void whenAMessageIsCreatedByAPublishMessageCommandThenItSendsAMessagePublishedEvent() {
        // Given
        String message = "message";
        PublishMessage publishMessage = new PublishMessage(message);

        SpyEventPublisher eventPublisher = new SpyEventPublisher();

        // When
        Message.publish(publishMessage, eventPublisher);

        // Then
        MessagePublished expectedEvent = new MessagePublished(new Message.MessageId(), message);
        assertThat(eventPublisher.publishedEvents).extracting("message").containsExactly(expectedEvent.getMessage());
    }

    @Test
    public void whenAMessageIsRepublishedThenItSendsAMessageRepublishedEvent() {
        // Given
        Message.MessageId messageId = new Message.MessageId();
        List<Event> eventHistory = history(new MessagePublished(messageId, "hello"));
        Message message = new Message(eventHistory);
        UserId userId = new UserId();

        SpyEventPublisher eventPublisher = new SpyEventPublisher();

        // When
        message.republish(userId, eventPublisher);

        // Then
        MessageRepublished expectedEvent = new MessageRepublished(messageId, userId);
        assertThat(eventPublisher.publishedEvents).extracting("messageId").containsExactly(expectedEvent.getMessageId());
        assertThat(eventPublisher.publishedEvents).extracting("userId").containsExactly(expectedEvent.getUserId());
    }

    @Test
    public void whenAMessageIsRepublishedTwiceByTheSameUserThenItShouldSendOnlyOneMessageRepublishedEvent() {
        // Given
        Message.MessageId messageId = new Message.MessageId();
        UserId userId = new UserId();
        List<Event> eventHistory = history(
                new MessagePublished(messageId, "hello"),
                new MessageRepublished(messageId, userId)
        );

        Message message = new Message(eventHistory);
        SpyEventPublisher eventPublisher = new SpyEventPublisher();

        // When
        message.republish(userId, eventPublisher);

        // Then
        assertThat(eventPublisher.publishedEvents).isEmpty();
    }

    public List<Event> history(Event... events) {
        List<Event> eventHistory = new ArrayList<>();
        Collections.addAll(eventHistory, events);
        return eventHistory;
    }

    class SpyEventPublisher implements EventPublisher {
        public List<Event> publishedEvents = new ArrayList<>();

        public void publish(Event event) {
            publishedEvents.add(event);
        }
    }
}

