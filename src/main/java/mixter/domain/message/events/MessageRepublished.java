package mixter.domain.message.events;

import mixter.Event;
import mixter.UserId;
import mixter.domain.message.MessageId;

public class MessageRepublished implements Event {
    private MessageId messageId;
    private UserId userId;

    public MessageRepublished(MessageId messageId, UserId userId) {
        this.messageId = messageId;
        this.userId = userId;
    }

    public MessageId getMessageId() {
        return messageId;
    }

    public UserId getUserId() {
        return userId;
    }
}
