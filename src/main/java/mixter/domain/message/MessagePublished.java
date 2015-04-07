package mixter.domain.message;

import mixter.Event;
import mixter.UserId;

public class MessagePublished implements Event {
    private final MessageId messageId;
    private final String message;
    private final UserId authorId;

    public MessagePublished(MessageId messageId, String message, UserId authorId) {
        this.messageId = messageId;
        this.message = message;
        this.authorId = authorId;
    }

    public MessageId getMessageId() {
        return messageId;
    }

    public String getMessage() {
        return message;
    }

    public UserId getAuthorId() {
        return authorId;
    }
}
