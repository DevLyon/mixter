package mixter;

public class MessageRepublished implements Event {
    private Message.MessageId messageId;
    private UserId userId;

    public MessageRepublished(Message.MessageId messageId, UserId userId) {
        this.messageId = messageId;
        this.userId = userId;
    }

    public Message.MessageId getMessageId() {
        return messageId;
    }

    public UserId getUserId() {
        return userId;
    }
}
