package mixter.domain.core.message;

import mixter.doc.Aggregate;
import mixter.doc.Projection;
import mixter.domain.DecisionProjectionBase;
import mixter.domain.Event;
import mixter.domain.EventPublisher;
import mixter.domain.core.message.events.MessageDeleted;
import mixter.domain.core.message.events.MessagePublished;
import mixter.domain.core.message.events.MessageRepublished;
import mixter.domain.core.message.events.ReplyMessagePublished;
import mixter.domain.identity.UserId;

import java.util.HashSet;
import java.util.List;
import java.util.Set;

@Aggregate
public class Message {
    private DecisionProjection projection;

    public Message(List<Event> history) {
        projection=new DecisionProjection(history);
    }

    public static MessageId publish(PublishMessage publishMessage, EventPublisher eventPublisher) {
        MessageId messageId = MessageId.generate();
        eventPublisher.publish(new MessagePublished(messageId, publishMessage.getMessage(), publishMessage.getAuthorId()));
        return messageId;
    }

    public void republish(UserId userId, EventPublisher eventPublisher, UserId authorId, String message) {
        if (projection.publishers.contains(userId)) {
            return;
        }
        MessageRepublished event = new MessageRepublished(projection.getId(), userId, authorId, message);
        eventPublisher.publish(event);
    }

    public MessageId reply(UserId replierId, MessageId messageId, UserId authorId, String replyContent, EventPublisher eventPublisher) {
        MessageId replyId = MessageId.generate();
        eventPublisher.publish(new ReplyMessagePublished(authorId, replierId, replyContent, messageId, replyId));
        return replyId;
    }

    public void delete(UserId authorId, EventPublisher eventPublisher) {
        if (projection.authorId == authorId && projection.isNotDeleted()) {
            eventPublisher.publish(new MessageDeleted(projection.id));
        }
    }

    @Projection
    private class DecisionProjection extends DecisionProjectionBase {
        private MessageId id;
        public Set<UserId> publishers=new HashSet<>();
        public UserId authorId;
        private boolean deleted;

        public DecisionProjection(List<Event> history) {
            super.register(MessagePublished.class, this::apply);
            super.register(MessageRepublished.class, this::apply);
            super.register(ReplyMessagePublished.class, this::apply);
            super.register(MessageDeleted.class, this::apply);
            history.forEach(this::apply);
        }

        private void apply(MessagePublished event) {
            id = event.getMessageId();
            publishers.add(event.getAuthorId());
            authorId = event.getAuthorId();
        }

        private void apply(ReplyMessagePublished event) {
            id = event.getMessageId();
            publishers.add(event.getAuthorId());
        }

        private void apply(MessageRepublished event) {
            publishers.add(event.getUserId());
        }

        private void apply(MessageDeleted event) {
            deleted = true;
        }

        public MessageId getId() {
            return id;
        }

        public boolean isNotDeleted() {
            return !deleted;
        }
    }
}
