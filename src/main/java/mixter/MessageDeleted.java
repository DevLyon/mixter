package mixter;

public class MessageDeleted implements Event {
    private Message.MessageId messageId;

    public MessageDeleted(Message.MessageId messageId) {
        this.messageId = messageId;
    }

    public Message.MessageId getMessageId() {
        return messageId;
    }
}
