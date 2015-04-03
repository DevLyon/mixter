package mixter;

import org.junit.Test;

import java.util.List;

import static org.assertj.core.api.Assertions.assertThat;

public class MessageTest extends AggregateTest {
    @Test
    public void whenAMessageIsCreatedByAPublishMessageCommandThenItSendsAMessagePublishedEvent() {
        // Given
        String message = "message";
        UserId authorId = new UserId();
        PublishMessage publishMessage = new PublishMessage(message, authorId);

        SpyEventPublisher eventPublisher = new SpyEventPublisher();

        // When
        Message.publish(publishMessage, eventPublisher);

        // Then
        MessagePublished expectedEvent = new MessagePublished(new Message.MessageId(), message, authorId);
        assertThat(eventPublisher.publishedEvents).extracting("message").containsExactly(expectedEvent.getMessage());
    }

    @Test
    public void whenAMessageIsRepublishedThenItSendsAMessageRepublishedEvent() {
        // Given
        Message.MessageId messageId = new Message.MessageId();
        UserId authorId = new UserId();
        List<Event> eventHistory = history(new MessagePublished(messageId, "hello", authorId));
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
    public void whenAMessageIsRepublishedByItsAuthorThenItShouldNotSendRepublishedEvent() {
        // Given
        Message.MessageId messageId = new Message.MessageId();
        UserId authorId = new UserId();
        List<Event> eventHistory = history(
                new MessagePublished(messageId, "hello", authorId)
        );

        Message message = new Message(eventHistory);
        SpyEventPublisher eventPublisher = new SpyEventPublisher();

        // When
        message.republish(authorId, eventPublisher);

        // Then
        assertThat(eventPublisher.publishedEvents).isEmpty();
    }

    @Test
    public void whenAMessageIsRepublishedTwiceByTheSameUserThenItShouldNotSendMessageRepublishedEvent() {
        // Given
        Message.MessageId messageId = new Message.MessageId();
        UserId userId = new UserId();
        UserId authorId = new UserId();
        List<Event> eventHistory = history(
                new MessagePublished(messageId, "hello", authorId),
                new MessageRepublished(messageId, userId)
        );

        Message message = new Message(eventHistory);
        SpyEventPublisher eventPublisher = new SpyEventPublisher();

        // When
        message.republish(userId, eventPublisher);

        // Then
        assertThat(eventPublisher.publishedEvents).isEmpty();
    }

    @Test
    public void whenAMessageIsDeletedByItsAuthorThenItShouldSendMessageDeletedEvent() {
        Message.MessageId messageId = new Message.MessageId();
        UserId authorId = new UserId();
        List<Event> eventHistory = history(
                new MessagePublished(messageId, "hello", authorId)
        );

        Message message = new Message(eventHistory);
        SpyEventPublisher eventPublisher = new SpyEventPublisher();

        // When
        message.delete(authorId, eventPublisher);

        // Then
        MessageDeleted expectedEvent = new MessageDeleted(messageId);
        assertThat(eventPublisher.publishedEvents).extracting("messageId").containsExactly(expectedEvent.getMessageId());
    }

    @Test
    public void whenAMessageIsDeletedBySomeoneElseThenItShouldNotSendMessageDeletedEvent() {
        Message.MessageId messageId = new Message.MessageId();
        UserId authorId = new UserId();
        List<Event> eventHistory = history(
                new MessagePublished(messageId, "hello", authorId)
        );

        Message message = new Message(eventHistory);
        SpyEventPublisher eventPublisher = new SpyEventPublisher();

        // When
        message.delete(new UserId(), eventPublisher);

        // Then
        assertThat(eventPublisher.publishedEvents).isEmpty();
    }

    @Test
    public void whenAMessageIsDeletedTwiceThenItShouldNotSendMessageDeletedEvent() {
        Message.MessageId messageId = new Message.MessageId();
        UserId authorId = new UserId();
        List<Event> eventHistory = history(
                new MessagePublished(messageId, "hello", authorId),
                new MessageDeleted(messageId)
        );

        Message message = new Message(eventHistory);
        SpyEventPublisher eventPublisher = new SpyEventPublisher();

        // When
        message.delete(authorId, eventPublisher);

        // Then
        assertThat(eventPublisher.publishedEvents).isEmpty();
    }

    @Test
    public void whenADeletedMessageIsRepublishedThenItShouldNotSendMessageRepublishedEvent() {
        Message.MessageId messageId = new Message.MessageId();
        UserId authorId = new UserId();
        List<Event> eventHistory = history(
                new MessagePublished(messageId, "hello", authorId),
                new MessageDeleted(messageId)
        );

        Message message = new Message(eventHistory);
        SpyEventPublisher eventPublisher = new SpyEventPublisher();

        // When
        message.republish(new UserId(), eventPublisher);

        // Then
        assertThat(eventPublisher.publishedEvents).isEmpty();
    }

}

