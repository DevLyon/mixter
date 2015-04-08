package mixter.domain.message;

public interface TimelineRepository {
    TimelineMessage save(TimelineMessage message);

    TimelineMessage getByMessageId(MessageId messageId);
}
