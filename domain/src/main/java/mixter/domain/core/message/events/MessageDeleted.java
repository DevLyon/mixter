package mixter.domain.core.message.events;

import mixter.domain.AggregateId;
import mixter.domain.Event;
import mixter.domain.core.message.MessageId;

public class MessageDeleted implements Event {
    private MessageId messageId;

    public MessageDeleted(MessageId messageId) {

        this.messageId = messageId;
    }

    @Override
    public AggregateId getId() {
        return messageId;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;

        MessageDeleted that = (MessageDeleted) o;

        return !(messageId != null ? !messageId.equals(that.messageId) : that.messageId != null);

    }

    @Override
    public int hashCode() {
        return messageId != null ? messageId.hashCode() : 0;
    }
}
