package mixter;

import java.util.UUID;

class Message{
    public static void publish(PublishMessage publishMessage, EventPublisher eventPublisher) {
        MessageId messageId = new MessageId();
        eventPublisher.publish(new MessagePublished(messageId, publishMessage.getMessage()));
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
