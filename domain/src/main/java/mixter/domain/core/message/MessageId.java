package mixter.domain.core.message;

import mixter.domain.AggregateId;

import java.util.UUID;

public class MessageId implements AggregateId {
    private String value;

    public MessageId(String value) {

        this.value = value;
    }

    public static MessageId generate() {
        return new MessageId(UUID.randomUUID().toString());
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;

        MessageId messageId = (MessageId) o;

        return !(value != null ? !value.equals(messageId.value) : messageId.value != null);

    }

    @Override
    public int hashCode() {
        return value != null ? value.hashCode() : 0;
    }
}
