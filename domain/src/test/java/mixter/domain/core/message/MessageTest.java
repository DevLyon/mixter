package mixter.domain.core.message;

import mixter.domain.Event;
import mixter.domain.SpyEventPublisher;
import mixter.domain.core.message.events.MessageDeleted;
import mixter.domain.core.message.events.MessagePublished;
import mixter.domain.core.message.events.MessageRepublished;
import mixter.domain.core.message.events.ReplyMessagePublished;
import mixter.domain.identity.UserId;
import org.junit.Before;
import org.junit.Test;

import java.util.List;

import static org.assertj.core.api.Assertions.assertThat;

public class MessageTest extends mixter.domain.DomainTest {
    public String CONTENT = "content";
    public UserId AUTHOR_ID = new UserId("author@mix-it.fr");
    public UserId USER_ID = new UserId("user@mix-it.fr");
    public UserId REPLIER_ID = new UserId("replier@mix-it.fr");
    public String REPLY_CONTENT = "reply content";
    public UserId RANDOM_GUY = new UserId("randomeGuy@mix-it.fr");
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
        assertThat(eventPublisher.publishedEvents).containsExactly(expectedEvent);
    }

    @Test
    public void whenAMessageIsRepublishedThenItSendsAMessageRepublishedEvent() {
        // Given
        MessageId messageId = MessageId.generate();
        Message message = messageFor(
                new MessagePublished(messageId, CONTENT, AUTHOR_ID)
        );

        // When
        message.republish(USER_ID, eventPublisher, AUTHOR_ID, CONTENT);

        // Then
        MessageRepublished expectedEvent = new MessageRepublished(messageId, USER_ID, AUTHOR_ID, CONTENT);
        assertThat(eventPublisher.publishedEvents).containsExactly(expectedEvent);
    }

    @Test
    public void whenAMessageIsRepublishedByItsAuthorThenItShouldNotSendRepublishedEvent() {
        // Given
        MessageId messageId = MessageId.generate();

        Message message = messageFor(
                new MessagePublished(messageId, CONTENT, AUTHOR_ID)
        );

        // When
        message.republish(AUTHOR_ID, eventPublisher, AUTHOR_ID, CONTENT);

        // Then
        assertThat(eventPublisher.publishedEvents).isEmpty();
    }

    @Test
    public void whenAMessageIsRepublishedTwiceByTheSameUserThenItShouldNotSendMessageRepublishedEvent() {
        // Given
        MessageId messageId = MessageId.generate();
        Message message = messageFor(
                new MessagePublished(messageId, CONTENT, AUTHOR_ID),
                new MessageRepublished(messageId, USER_ID, AUTHOR_ID, CONTENT)
        );

        // When
        message.republish(USER_ID, eventPublisher, AUTHOR_ID, CONTENT);

        // Then
        assertThat(eventPublisher.publishedEvents).isEmpty();
    }

    @Test
    public void whenReplyThenRaiseReplyMessagePublished() {
        MessageId originalMessageId = MessageId.generate();
        Message message = messageFor(
                new MessagePublished(originalMessageId, CONTENT, AUTHOR_ID)
        );

        // When
        MessageId replyId = message.reply(REPLIER_ID, originalMessageId, AUTHOR_ID, REPLY_CONTENT, eventPublisher);

        //Then
        ReplyMessagePublished expected = new ReplyMessagePublished(AUTHOR_ID, REPLIER_ID, REPLY_CONTENT, originalMessageId, replyId);
        assertThat(eventPublisher.publishedEvents).containsExactly(expected);
    }

    @Test
    public void whenRepublishReplyThenRaiseMessageRepublishedWithReplyIdAsMessageId() {
        MessageId originalMessageId = MessageId.generate();
        MessageId replyId = MessageId.generate();
        Message message = messageFor(
                new ReplyMessagePublished(REPLIER_ID, AUTHOR_ID, REPLY_CONTENT, originalMessageId, replyId)
        );

        // When
        message.republish(USER_ID, eventPublisher, REPLIER_ID, REPLY_CONTENT);

        //Then
        MessageRepublished expectedEvent = new MessageRepublished(replyId, USER_ID, REPLIER_ID, REPLY_CONTENT);
        assertThat(eventPublisher.publishedEvents).containsExactly(expectedEvent);
    }


    public void whenAMessageIsDeletedByItsAuthorThenItShouldSendMessageDeletedEvent() {
        // Given
        MessageId messageId = MessageId.generate();
        List<Event> eventHistory = history(
                new MessagePublished(messageId, CONTENT, AUTHOR_ID)
        );

        Message message = new Message(eventHistory);

        // When
        message.delete(AUTHOR_ID, eventPublisher);

        // Then
        MessageDeleted expectedEvent = new MessageDeleted(messageId);
        assertThat(eventPublisher.publishedEvents).containsExactly(expectedEvent);
    }

    @Test
    public void whenAMessageIsDeletedBySomeoneElseThenItShouldNotSendMessageDeletedEvent() {
        // Given
        MessageId messageId = MessageId.generate();
        List<Event> eventHistory = history(
                new MessagePublished(messageId, CONTENT, AUTHOR_ID)
        );

        Message message = new Message(eventHistory);

        // When
        message.delete(RANDOM_GUY, eventPublisher);

        // Then
        assertThat(eventPublisher.publishedEvents).isEmpty();
    }

    @Test
    public void whenAMessageIsDeletedTwiceThenItShouldNotSendMessageDeletedEvent() {
        // Given
        MessageId messageId = MessageId.generate();
        Message message = messageFor(
                new MessagePublished(messageId, CONTENT, AUTHOR_ID),
                new MessageDeleted(messageId)
        );

        // When
        message.delete(AUTHOR_ID, eventPublisher);

        // Then
        assertThat(eventPublisher.publishedEvents).isEmpty();
    }

    @Test
    public void whenADeletedMessageIsRepublishedThenItShouldNotSendMessageRepublishedEvent() {
        // Given
        MessageId messageId = MessageId.generate();
        Message message = messageFor(
                new MessagePublished(messageId, CONTENT, AUTHOR_ID),
                new MessageDeleted(messageId)
        );

        // When
        message.republish(RANDOM_GUY, eventPublisher, AUTHOR_ID, CONTENT);

        // Then
        assertThat(eventPublisher.publishedEvents).isEmpty();
    }

    @Test
    public void whenADeletedMessageIsRepiedToThenItShouldNotSendReplyMessagePublishedEvent() {
        // Given
        MessageId originalMessageId = MessageId.generate();
        Message message = messageFor(
                new MessagePublished(originalMessageId, CONTENT, AUTHOR_ID),
                new MessageDeleted(originalMessageId)
        );

        // When
        MessageId replyId = message.reply(REPLIER_ID, originalMessageId, AUTHOR_ID, REPLY_CONTENT, eventPublisher);

        // Then
        assertThat(eventPublisher.publishedEvents).isEmpty();
    }

    protected Message messageFor(Event... events) {
        return new Message(history(events));
    }
}
