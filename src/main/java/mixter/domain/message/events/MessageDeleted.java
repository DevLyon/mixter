package mixter.domain.message.events;

import mixter.Event;
import mixter.domain.message.MessageId;

public class MessageDeleted implements Event {
    private MessageId messageId;

    public MessageDeleted(MessageId messageId) {
        this.messageId = messageId;
    }

    public MessageId getMessageId() {
        return messageId;
    }
}
