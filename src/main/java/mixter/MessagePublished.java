package mixter;

class MessagePublished implements Event{
    private final Message.MessageId messageId;
    private final String message;
    private final UserId authorId;

    MessagePublished(Message.MessageId messageId, String message, UserId authorId) {
        this.messageId = messageId;
        this.message = message;
        this.authorId = authorId;
    }

    public Message.MessageId getMessageId() {
        return messageId;
    }

    public String getMessage() {
        return message;
    }

    public UserId getAuthorId() {
        return authorId;
    }
}
