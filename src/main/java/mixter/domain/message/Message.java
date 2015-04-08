package mixter.domain.message;

import mixter.Event;
import mixter.EventPublisher;
import mixter.UserId;
import mixter.domain.message.events.MessageDeleted;
import mixter.domain.message.events.MessagePublished;
import mixter.domain.message.events.MessageReplied;
import mixter.domain.message.events.MessageRepublished;

import java.util.*;
import java.util.function.Consumer;

class Message {
    private final DecisionProjection projection;

    public Message(List<Event> eventHistory) {
        projection = new DecisionProjection(eventHistory);
    }

    public static void publish(PublishMessage publishMessage, EventPublisher eventPublisher) {
        MessageId messageId = new MessageId();
        eventPublisher.publish(new MessagePublished(messageId, publishMessage.getMessage(), publishMessage.getAuthorId()));
    }

    public void republish(UserId userId, EventPublisher eventPublisher, UserId authorId, String content) {
        if (projection.publishers.contains(userId) || projection.isDeleted()) {
            return;
        }
        MessageRepublished event = new MessageRepublished(projection.getId(), userId, authorId, content);
        eventPublisher.publish(event);
        projection.apply(event);
    }

    public void delete(UserId authorId, EventPublisher eventPublisher) {
        if (projection.getAuthorId() == authorId && projection.isNotDeleted()) {
            eventPublisher.publish(new MessageDeleted(projection.getId()));
        }
    }

    public void reply(UserId replierId, MessageId messageId, UserId authorId, String replyContent, EventPublisher eventPublisher) {
        if (projection.isNotDeleted()) {
            eventPublisher.publish(new MessageReplied(authorId, replierId, replyContent, messageId, new MessageId()));
        }
    }

    MessageId getId() {
        return projection.getId();
    }

    private class DecisionProjection {
        private MessageId id;
        private Map<Class, Consumer> appliers = new HashMap<>();

        public Set<UserId> publishers = new HashSet<>();
        private UserId authorId;
        private boolean deleted = false;

        public DecisionProjection(List<Event> eventHistory) {
            Consumer<MessagePublished> applyMessagePublished = this::apply;
            Consumer<MessageRepublished> applyMessageRepublished = this::apply;
            Consumer<MessageDeleted> applyMessageDeleted = this::apply;
            Consumer<MessageReplied> applyMessageReplied = this::apply;
            appliers.put(MessagePublished.class, applyMessagePublished);
            appliers.put(MessageRepublished.class, applyMessageRepublished);
            appliers.put(MessageDeleted.class, applyMessageDeleted);
            appliers.put(MessageReplied.class, applyMessageReplied);
            eventHistory.forEach(this::apply);
        }

        public MessageId getId() {
            return id;
        }

        @SuppressWarnings("unchecked")
        public void apply(Event event) {
            Consumer consumer = appliers.get(event.getClass());
            consumer.accept(event);
        }

        private void apply(MessagePublished event) {
            id = event.getMessageId();
            authorId = event.getAuthorId();
            publishers.add(event.getAuthorId());
        }

        private void apply(MessageRepublished event) {
            publishers.add(event.getUserId());
        }

        private void apply(MessageDeleted event) {
            deleted = true;
        }

        private void apply(MessageReplied event) {
            id = event.getMessageId();
            authorId = event.getAuthorId();
            publishers.add(event.getAuthorId());
        }

        public UserId getAuthorId() {
            return authorId;
        }

        public boolean isNotDeleted() {
            return !deleted;
        }

        public boolean isDeleted() {
            return deleted;
        }
    }

}
