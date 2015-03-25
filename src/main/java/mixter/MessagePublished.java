package mixter;

class MessagePublished implements Event{
    private final Message.MessageId messageId;
    private final String message;
    MessagePublished(Message.MessageId messageId, String message) {
        this.messageId = messageId;
        this.message = message;
    }

    public Message.MessageId getMessageId() {
        return messageId;
    }

    public String getMessage() {
        return message;
    }
}
