package mixter.domain.message;

import mixter.AggregateTest;
import mixter.Event;
import mixter.UserId;
import mixter.domain.message.events.MessageDeleted;
import mixter.domain.message.events.MessagePublished;
import mixter.domain.message.events.MessageRepublished;
import org.junit.Before;
import org.junit.Test;

import java.util.List;

import static org.assertj.core.api.Assertions.assertThat;

public class MessageTest extends AggregateTest {

    public static final UserId AUTHOR_ID = new UserId();
    public static final String CONTENT = "hello";
    public static final UserId USER_ID = new UserId();
    private SpyEventPublisher eventPublisher;

    @Before
    public void initialize() {
        eventPublisher = new SpyEventPublisher();
    }

    @Test
    public void whenAMessageIsCreatedByAPublishMessageCommandThenItSendsAMessagePublishedEvent() {
        // Given
        PublishMessage publishMessage = new PublishMessage(CONTENT, AUTHOR_ID);

        // When
        Message.publish(publishMessage, eventPublisher);

        // Then
        MessagePublished expectedEvent = new MessagePublished(new MessageId(), CONTENT, AUTHOR_ID);
        assertThat(eventPublisher.publishedEvents).extracting("message").containsExactly(expectedEvent.getMessage());
    }

    @Test
    public void whenAMessageIsRepublishedThenItSendsAMessageRepublishedEvent() {
        // Given
        MessageId messageId = new MessageId();
        List<Event> eventHistory = history(new MessagePublished(messageId, CONTENT, AUTHOR_ID));
        Message message = new Message(eventHistory);
        UserId userId = new UserId();

        // When
        message.republish(userId, eventPublisher, AUTHOR_ID, CONTENT);

        // Then
        MessageRepublished expectedEvent = new MessageRepublished(messageId, userId, AUTHOR_ID, CONTENT);
        assertThat(eventPublisher.publishedEvents).extracting("messageId").containsExactly(expectedEvent.getMessageId());
        assertThat(eventPublisher.publishedEvents).extracting("userId").containsExactly(expectedEvent.getUserId());
    }

    @Test
    public void whenAMessageIsRepublishedByItsAuthorThenItShouldNotSendRepublishedEvent() {
        // Given
        MessageId messageId = new MessageId();
        List<Event> eventHistory = history(
                new MessagePublished(messageId, CONTENT, AUTHOR_ID)
        );

        Message message = new Message(eventHistory);

        // When
        message.republish(AUTHOR_ID, eventPublisher, AUTHOR_ID, CONTENT);

        // Then
        assertThat(eventPublisher.publishedEvents).isEmpty();
    }

    @Test
    public void whenAMessageIsRepublishedTwiceByTheSameUserThenItShouldNotSendMessageRepublishedEvent() {
        // Given
        MessageId messageId = new MessageId();
        List<Event> eventHistory = history(
                new MessagePublished(messageId, CONTENT, AUTHOR_ID),
                new MessageRepublished(messageId, USER_ID, AUTHOR_ID, CONTENT)
        );

        Message message = new Message(eventHistory);

        // When
        message.republish(USER_ID, eventPublisher, AUTHOR_ID, CONTENT);

        // Then
        assertThat(eventPublisher.publishedEvents).isEmpty();
    }

    @Test
    public void whenAMessageIsDeletedByItsAuthorThenItShouldSendMessageDeletedEvent() {
        // Given
        MessageId messageId = new MessageId();
        List<Event> eventHistory = history(
                new MessagePublished(messageId, CONTENT, AUTHOR_ID)
        );

        Message message = new Message(eventHistory);

        // When
        message.delete(AUTHOR_ID, eventPublisher);

        // Then
        MessageDeleted expectedEvent = new MessageDeleted(messageId);
        assertThat(eventPublisher.publishedEvents).extracting("messageId").containsExactly(expectedEvent.getMessageId());
    }

    @Test
    public void whenAMessageIsDeletedBySomeoneElseThenItShouldNotSendMessageDeletedEvent() {
// Given
        MessageId messageId = new MessageId();
        List<Event> eventHistory = history(
                new MessagePublished(messageId, CONTENT, AUTHOR_ID)
        );

        Message message = new Message(eventHistory);

        // When
        message.delete(new UserId(), eventPublisher);

        // Then
        assertThat(eventPublisher.publishedEvents).isEmpty();
    }

    @Test
    public void whenAMessageIsDeletedTwiceThenItShouldNotSendMessageDeletedEvent() {
        // Given
        MessageId messageId = new MessageId();
        List<Event> eventHistory = history(
                new MessagePublished(messageId, CONTENT, AUTHOR_ID),
                new MessageDeleted(messageId)
        );

        Message message = new Message(eventHistory);

        // When
        message.delete(AUTHOR_ID, eventPublisher);

        // Then
        assertThat(eventPublisher.publishedEvents).isEmpty();
    }

    @Test
    public void whenADeletedMessageIsRepublishedThenItShouldNotSendMessageRepublishedEvent() {
        // Given
        MessageId messageId = new MessageId();
        List<Event> eventHistory = history(
                new MessagePublished(messageId, CONTENT, AUTHOR_ID),
                new MessageDeleted(messageId)
        );

        Message message = new Message(eventHistory);

        // When
        message.republish(new UserId(), eventPublisher, AUTHOR_ID, CONTENT);

        // Then
        assertThat(eventPublisher.publishedEvents).isEmpty();
    }
}

