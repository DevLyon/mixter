package mixter.domain.message;

import mixter.Event;

class MessageDeleted implements Event {
    private MessageId messageId;

    public MessageDeleted(MessageId messageId) {
        this.messageId = messageId;
    }

    public MessageId getMessageId() {
        return messageId;
    }
}
