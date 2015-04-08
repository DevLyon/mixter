package mixter.domain.message.events;

import mixter.Event;
import mixter.UserId;
import mixter.domain.message.MessageId;

public class MessageReplied implements Event {
    private final UserId authorId;
    private final UserId replierId;
    private final String message;
    private final MessageId originalMessageId;
    private final MessageId messageId;

    public MessageReplied(UserId authorId, UserId replierId, String message, MessageId originalMessageId, MessageId messageId) {
        this.authorId = authorId;
        this.replierId = replierId;
        this.message = message;
        this.originalMessageId = originalMessageId;
        this.messageId = messageId;
    }

    public UserId getAuthorId() {
        return authorId;
    }

    public UserId getReplierId() {
        return replierId;
    }

    public String getMessage() {
        return message;
    }

    public MessageId getMessageId() {
        return messageId;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;

        MessageReplied that = (MessageReplied) o;

        if (authorId != null ? !authorId.equals(that.authorId) : that.authorId != null)
            return false;
        if (replierId != null ? !replierId.equals(that.replierId) : that.replierId != null)
            return false;
        if (message != null ? !message.equals(that.message) : that.message != null)
            return false;
        return !(messageId != null ? !messageId.equals(that.messageId) : that.messageId != null);

    }

    @Override
    public int hashCode() {
        int result = authorId != null ? authorId.hashCode() : 0;
        result = 31 * result + (replierId != null ? replierId.hashCode() : 0);
        result = 31 * result + (message != null ? message.hashCode() : 0);
        result = 31 * result + (messageId != null ? messageId.hashCode() : 0);
        return result;
    }

    public MessageId getOriginalMessageId() {
        return originalMessageId;
    }
}
