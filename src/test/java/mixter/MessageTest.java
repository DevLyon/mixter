package mixter;

import org.junit.Test;

import java.util.ArrayList;
import java.util.List;

import static org.assertj.core.api.Assertions.assertThat;

public class MessageTest {
    @Test
    public void whenAMessageIsCreatedByAPublishMessageCommandThenItSendsAMessagePublishedEvent() {
        // Given
        String message = "message";
        PublishMessage publishMessage= new PublishMessage(message);

        final List<Event> publishedEvents = new ArrayList<Event>();
        EventPublisher eventPublisher= new EventPublisher(){
            public void publish(Event event){
                publishedEvents.add(event);
            }
        };

        // When
        Message.publish(publishMessage, eventPublisher);

        // Then
        MessagePublished expectedEvent = new MessagePublished(new Message.MessageId(),message);
        assertThat(publishedEvents).extracting("message").containsExactly(expectedEvent .getMessage());
    }

    @Test
    public void whenAMessageIsRepublishedThenItSendsAMessageRepublishedEvent() {
        // Given
        List<Event> eventHistory = new ArrayList<Event>();
        Message.MessageId messageId = new Message.MessageId();
        eventHistory.add(new MessagePublished(messageId, "hello"));
        Message message = new Message(eventHistory);

        final List<Event> publishedEvents = new ArrayList<Event>();
        EventPublisher eventPublisher= new EventPublisher(){
            public void publish(Event event){
                publishedEvents.add(event);
            }
        };

        // When
        message.republish(eventPublisher);

        // Then
        MessageRepublished expectedEvent = new MessageRepublished(messageId);
        assertThat(publishedEvents).extracting("messageId").containsExactly(expectedEvent.getMessageId());
    }
}

