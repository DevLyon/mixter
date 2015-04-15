package mixter.domain.core.message;

import mixter.domain.identity.UserId;

public class TimelineMessageProjection {
    private final UserId ownerId;
    private final UserId authorId;
    private final String content;
    private final MessageId messageId;

    public TimelineMessageProjection(UserId ownerId, UserId authorId, String content, MessageId messageId) {
        this.ownerId = ownerId;
        this.authorId = authorId;
        this.content = content;
        this.messageId = messageId;
    }

    public MessageId getMessageId() {
        return messageId;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;

        TimelineMessageProjection that = (TimelineMessageProjection) o;

        if (ownerId != null ? !ownerId.equals(that.ownerId) : that.ownerId != null) return false;
        if (authorId != null ? !authorId.equals(that.authorId) : that.authorId != null) return false;
        if (content != null ? !content.equals(that.content) : that.content != null) return false;
        return !(messageId != null ? !messageId.equals(that.messageId) : that.messageId != null);

    }

    @Override
    public int hashCode() {
        int result = ownerId != null ? ownerId.hashCode() : 0;
        result = 31 * result + (authorId != null ? authorId.hashCode() : 0);
        result = 31 * result + (content != null ? content.hashCode() : 0);
        result = 31 * result + (messageId != null ? messageId.hashCode() : 0);
        return result;
    }
}
