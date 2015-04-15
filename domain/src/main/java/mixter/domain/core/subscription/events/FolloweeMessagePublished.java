package mixter.domain.core.subscription.events;

import mixter.domain.AggregateId;
import mixter.domain.Event;
import mixter.domain.core.message.MessageId;
import mixter.domain.core.subscription.SubscriptionId;

public class FolloweeMessagePublished implements Event {
    private final SubscriptionId subscriptionId;
    private final MessageId messageId;

    public FolloweeMessagePublished(SubscriptionId subscriptionId, MessageId messageId) {
        this.subscriptionId = subscriptionId;
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

        FolloweeMessagePublished that = (FolloweeMessagePublished) o;

        if (subscriptionId != null ? !subscriptionId.equals(that.subscriptionId) : that.subscriptionId != null)
            return false;
        return !(messageId != null ? !messageId.equals(that.messageId) : that.messageId != null);

    }

    @Override
    public int hashCode() {
        int result = subscriptionId != null ? subscriptionId.hashCode() : 0;
        result = 31 * result + (messageId != null ? messageId.hashCode() : 0);
        return result;
    }
}
