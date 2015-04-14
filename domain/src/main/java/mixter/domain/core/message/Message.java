package mixter.domain.core.message;

import mixter.domain.Event;
import mixter.domain.EventPublisher;
import mixter.domain.UserId;
import mixter.domain.core.message.events.MessageQuacked;
import mixter.domain.core.message.events.MessageRequacked;

import java.util.*;
import java.util.function.Consumer;

public class Message {
    private DecisionProjection projection;

    public Message(List<Event> history) {
        projection=new DecisionProjection(history);
    }

    public static MessageId quack(UserId authorId, String message, EventPublisher eventPublisher) {
        MessageId messageId = new MessageId();
        eventPublisher.publish(new MessageQuacked(messageId, message, authorId));
        return messageId;
    }

    public void reQuack(UserId userId, EventPublisher eventPublisher, UserId authorId, String message) {
        if (projection.publishers.contains(userId)) {
            return;
        }
        MessageRequacked event = new MessageRequacked(projection.getId(), userId, authorId, message);
        eventPublisher.publish(event);
    }

    private class DecisionProjection {
        private MessageId id;
        private Map<Class, Consumer> appliers = new HashMap<>();
        public Set<UserId> publishers=new HashSet<>();

        public DecisionProjection(List<Event> history) {
            Consumer<MessageQuacked> applyMessagePublished = this::apply;
            Consumer<MessageRequacked> applyMessageRepublished = this::apply;
            appliers.put(MessageQuacked.class, applyMessagePublished);
            appliers.put(MessageRequacked.class, applyMessageRepublished);
            history.forEach(this::apply);
        }
        @SuppressWarnings("unchecked")
        public void apply(Event event) {
            Consumer consumer = appliers.get(event.getClass());
            consumer.accept(event);
        }

        private void apply(MessageQuacked event) {
            id = event.getMessageId();
            publishers.add(event.getAuthorId());
        }

        private void apply(MessageRequacked event) {
            publishers.add(event.getUserId());
        }

        public MessageId getId() {
            return id;
        }
    }
}
