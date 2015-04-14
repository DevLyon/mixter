package mixter.domain.core.message;

import mixter.domain.EventPublisher;
import mixter.domain.core.message.events.MessagePublished;

public class Message {
    public static MessageId publish(PublishMessage publishMessage, EventPublisher eventPublisher) {
        MessageId messageId = new MessageId();
        eventPublisher.publish(new MessagePublished(messageId, publishMessage.getMessage(), publishMessage.getAuthorId()));
        return messageId;
    }
}
