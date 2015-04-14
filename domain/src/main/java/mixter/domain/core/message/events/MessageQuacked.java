package mixter.domain.core.message.events;

import mixter.domain.UserId;
import mixter.domain.core.message.MessageId;

public class MessageQuacked {
    private final MessageId messageId;
    private final String message;
    private final UserId authorId;

    public MessageQuacked(MessageId messageId, String message, UserId authorId) {
        this.messageId = messageId;
        this.message = message;
        this.authorId = authorId;
    }

    public String getMessage() {
        return message;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;

        MessageQuacked that = (MessageQuacked) o;

        if (messageId != null ? !messageId.equals(that.messageId) : that.messageId != null) return false;
        if (message != null ? !message.equals(that.message) : that.message != null) return false;
        return !(authorId != null ? !authorId.equals(that.authorId) : that.authorId != null);

    }

    @Override
    public int hashCode() {
        int result = messageId != null ? messageId.hashCode() : 0;
        result = 31 * result + (message != null ? message.hashCode() : 0);
        result = 31 * result + (authorId != null ? authorId.hashCode() : 0);
        return result;
    }
}
