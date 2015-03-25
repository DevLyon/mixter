package mixter;

import java.util.List;
import java.util.UUID;

class Message{
    private final DecisionProjection projection;

    public Message(List<Event> eventHistory) {
        projection=new DecisionProjection((MessagePublished)eventHistory.get(0));
    }

    public static void publish(PublishMessage publishMessage, EventPublisher eventPublisher) {
        MessageId messageId = new MessageId();
        eventPublisher.publish(new MessagePublished(messageId, publishMessage.getMessage()));
    }

    public void republish(EventPublisher eventPublisher) {
        eventPublisher.publish(new MessageRepublished(projection.getId()));
    }

    private class DecisionProjection{
        private MessageId id;
        public DecisionProjection(MessagePublished event) {
            id = event.getMessageId();
        }

        public MessageId getId() {
            return id;
        }
    }

    public static class MessageId {
        private final String id;
        public MessageId() {
            this.id = UUID.randomUUID().toString();
        }

        @Override
        public boolean equals(Object o) {
            if (this == o) return true;
            if (o == null || getClass() != o.getClass()) return false;

            MessageId messageId = (MessageId) o;

            return id.equals(messageId.id);

        }

        @Override
        public int hashCode() {
            return id.hashCode();
        }

        @Override
        public String toString() {
            return id;
        }

        public String getId() {
            return id;
        }
    }
}
