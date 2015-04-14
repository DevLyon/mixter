package mixter.domain.core.message;

import mixter.domain.Event;
import mixter.domain.EventPublisher;
import mixter.domain.UserId;
import mixter.domain.core.message.events.MessagePublished;
import mixter.domain.core.message.events.MessageRepublished;

import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.function.Consumer;

public class Message {
    private DecisionProjection projection;

    public Message(List<MessagePublished> history) {
        projection=new DecisionProjection(history);
    }

    public static MessageId publish(PublishMessage publishMessage, EventPublisher eventPublisher) {
        MessageId messageId = new MessageId();
        eventPublisher.publish(new MessagePublished(messageId, publishMessage.getMessage(), publishMessage.getAuthorId()));
        return messageId;
    }

    public void republish(UserId userId, EventPublisher eventPublisher, UserId authorId, String message) {
        MessageRepublished event = new MessageRepublished(projection.getId(), userId, authorId, message);
        eventPublisher.publish(event);
    }

    private class DecisionProjection {
        private MessageId id;
        private Map<Class, Consumer> appliers = new HashMap<>();

        public DecisionProjection(List<MessagePublished> history) {
            Consumer<MessagePublished> applyMessagePublished = this::apply;
            appliers.put(MessagePublished.class, applyMessagePublished);
            history.forEach(this::apply);
        }
        @SuppressWarnings("unchecked")
        public void apply(Event event) {
            Consumer consumer = appliers.get(event.getClass());
            consumer.accept(event);
        }

        private void apply(MessagePublished event) {
            id = event.getMessageId();
        }

        public MessageId getId() {
            return id;
        }
    }
}
