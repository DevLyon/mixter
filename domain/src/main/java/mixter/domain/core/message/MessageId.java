package mixter.domain.core.message;

import mixter.domain.AggregateId;

public class MessageId implements AggregateId {
    public static MessageId generate() {
        return new MessageId();
    }
}
