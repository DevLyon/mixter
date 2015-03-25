package mixter;

public class MessageRepublished implements Event {
    private Message.MessageId messageId;

    public MessageRepublished(Message.MessageId messageId) {
        this.messageId = messageId;
    }

    public Message.MessageId getMessageId() {
        return messageId;
    }
}
