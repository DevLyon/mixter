package mixter.domain.core.message;

import mixter.domain.Event;
import mixter.domain.EventPublisher;
import mixter.domain.UserId;
import mixter.domain.core.message.events.MessageQuacked;
import mixter.domain.core.message.events.MessageRequacked;

import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.function.Consumer;

public class Message {
    private DecisionProjection projection;

    public Message(List<MessageQuacked> history) {
        projection=new DecisionProjection(history);
    }

    public static MessageId quack(UserId authorId, String message, EventPublisher eventPublisher) {
        MessageId messageId = new MessageId();
        eventPublisher.publish(new MessageQuacked(messageId, message, authorId));
        return messageId;
    }

    public void reQuack(UserId userId, EventPublisher eventPublisher, UserId authorId, String message) {
        MessageRequacked event = new MessageRequacked(projection.getId(), userId, authorId, message);
        eventPublisher.publish(event);
    }

    private class DecisionProjection {
        private MessageId id;
        private Map<Class, Consumer> appliers = new HashMap<>();

        public DecisionProjection(List<MessageQuacked> history) {
            Consumer<MessageQuacked> applyMessagePublished = this::apply;
            appliers.put(MessageQuacked.class, applyMessagePublished);
            history.forEach(this::apply);
        }
        @SuppressWarnings("unchecked")
        public void apply(Event event) {
            Consumer consumer = appliers.get(event.getClass());
            consumer.accept(event);
        }

        private void apply(MessageQuacked event) {
            id = event.getMessageId();
        }

        public MessageId getId() {
            return id;
        }
    }
}
