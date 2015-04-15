package mixter.domain.core.message.events;

import mixter.domain.AggregateId;
import mixter.domain.Event;
import mixter.domain.core.message.MessageId;
import mixter.domain.identity.UserId;

public class ReplyMessagePublished implements Event {
    private final UserId authorId;
    private final UserId replierId;
    private final String replyContent;
    private final MessageId originalMessageId;
    private final MessageId messageId;

    public ReplyMessagePublished(UserId authorId, UserId replierId, String replyContent, MessageId originalMessageId, MessageId messageId) {
        this.authorId = authorId;
        this.replierId = replierId;
        this.replyContent = replyContent;
        this.originalMessageId = originalMessageId;
        this.messageId = messageId;
    }

    @Override
    public AggregateId getId() {
        return messageId;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;

        ReplyMessagePublished that = (ReplyMessagePublished) o;

        if (authorId != null ? !authorId.equals(that.authorId) : that.authorId != null) return false;
        if (replierId != null ? !replierId.equals(that.replierId) : that.replierId != null) return false;
        if (replyContent != null ? !replyContent.equals(that.replyContent) : that.replyContent != null) return false;
        if (originalMessageId != null ? !originalMessageId.equals(that.originalMessageId) : that.originalMessageId != null)
            return false;
        return !(messageId != null ? !messageId.equals(that.messageId) : that.messageId != null);

    }

    @Override
    public int hashCode() {
        int result = authorId != null ? authorId.hashCode() : 0;
        result = 31 * result + (replierId != null ? replierId.hashCode() : 0);
        result = 31 * result + (replyContent != null ? replyContent.hashCode() : 0);
        result = 31 * result + (originalMessageId != null ? originalMessageId.hashCode() : 0);
        result = 31 * result + (messageId != null ? messageId.hashCode() : 0);
        return result;
    }
}
