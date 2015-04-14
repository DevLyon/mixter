package mixter.domain.core.message;

import mixter.domain.DecisionProjectionBase;
import mixter.domain.Event;
import mixter.domain.EventPublisher;
import mixter.domain.core.message.events.MessagePublished;
import mixter.domain.core.message.events.MessageRepublished;
import mixter.domain.identity.UserId;

import java.util.HashSet;
import java.util.List;
import java.util.Set;
import java.util.function.Consumer;

public class Message {
    private DecisionProjection projection;

    public Message(List<Event> history) {
        projection=new DecisionProjection(history);
    }

    public static MessageId publish(PublishMessage publishMessage, EventPublisher eventPublisher) {
        MessageId messageId = new MessageId();
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

    private class DecisionProjection extends DecisionProjectionBase {
        private MessageId id;
        public Set<UserId> publishers=new HashSet<>();

        public DecisionProjection(List<Event> history) {
            Consumer<MessagePublished> applyMessagePublished = this::apply;
            Consumer<MessageRepublished> applyMessageRepublished = this::apply;
            super.register(MessagePublished.class, applyMessagePublished);
            super.register(MessageRepublished.class, applyMessageRepublished);
            history.forEach(this::apply);
        }

        private void apply(MessagePublished event) {
            id = event.getMessageId();
            publishers.add(event.getAuthorId());
        }

        private void apply(MessageRepublished event) {
            publishers.add(event.getUserId());
        }

        public MessageId getId() {
            return id;
        }
    }
}
