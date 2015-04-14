package mixter.domain.core.message;

import mixter.domain.EventPublisher;
import mixter.domain.UserId;
import mixter.domain.core.message.events.MessageQuacked;

public class Message {
    public static MessageId quack(UserId authorId, String message, EventPublisher eventPublisher) {
        MessageId messageId = new MessageId();
        eventPublisher.publish(new MessageQuacked(messageId, message, authorId));
        return messageId;
    }
}
