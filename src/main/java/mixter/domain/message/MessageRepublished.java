package mixter.domain.message;

import mixter.Event;
import mixter.UserId;

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
