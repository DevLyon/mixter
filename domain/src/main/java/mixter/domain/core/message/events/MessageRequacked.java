package mixter.domain.core.message.events;

import mixter.domain.Event;
import mixter.domain.UserId;
import mixter.domain.core.message.MessageId;

public class MessageRequacked implements Event {
    private final MessageId messageId;
    private final UserId userId;
    private final UserId authorId;
    private final String message;

    public MessageRequacked(MessageId messageId, UserId userId, UserId authorId, String message) {
        this.messageId = messageId;
        this.userId = userId;
        this.authorId = authorId;
        this.message = message;
    }

    public MessageId getMessageId() {
        return messageId;
    }

    public UserId getUserId() {
        return userId;
    }

    public UserId getAuthorId() {
        return authorId;
    }

    public String getMessage() {
        return message;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;

        MessageRequacked that = (MessageRequacked) o;

        if (messageId != null ? !messageId.equals(that.messageId) : that.messageId != null) return false;
        if (userId != null ? !userId.equals(that.userId) : that.userId != null) return false;
        if (authorId != null ? !authorId.equals(that.authorId) : that.authorId != null) return false;
        return !(message != null ? !message.equals(that.message) : that.message != null);

    }

    @Override
    public int hashCode() {
        int result = messageId != null ? messageId.hashCode() : 0;
        result = 31 * result + (userId != null ? userId.hashCode() : 0);
        result = 31 * result + (authorId != null ? authorId.hashCode() : 0);
        result = 31 * result + (message != null ? message.hashCode() : 0);
        return result;
    }
}
