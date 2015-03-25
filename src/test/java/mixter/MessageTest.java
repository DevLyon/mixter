package mixter;

import org.junit.Test;

import java.util.ArrayList;
import java.util.List;

import static org.assertj.core.api.Assertions.assertThat;

public class MessageTest {
    @Test
    public void whenAMessageIsCreatedByAPublishMessageCommandThenItSendsAMessagePublishedEvent() {
        final List<Event> publishedEvents = new ArrayList<Event>();
        //Given
        String message = "message";
        PublishMessage publishMessage= new PublishMessage(message);
        MessagePublished messagePublished=new MessagePublished(new Message.MessageId(),message);

        EventPublisher eventPublisher= new EventPublisher(){
            public void publish(Event event){
                publishedEvents.add(event);
            }
        };
        // When
        Message.publish(publishMessage, eventPublisher);
        // Then
        assertThat(publishedEvents).extracting("message").containsExactly(messagePublished.getMessage());
    }
}

