package mixter.domain.core.subscription.events;

import mixter.domain.AggregateId;
import mixter.domain.Event;
import mixter.domain.core.subscription.SubscriptionId;

public class UserUnfollowed implements Event {
    private SubscriptionId subscriptionId;

    public UserUnfollowed(SubscriptionId subscriptionId) {
        this.subscriptionId = subscriptionId;
    }

    @Override
    public AggregateId getId() {
        return subscriptionId;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;

        UserUnfollowed that = (UserUnfollowed) o;

        return !(subscriptionId != null ? !subscriptionId.equals(that.subscriptionId) : that.subscriptionId != null);

    }

    @Override
    public int hashCode() {
        return subscriptionId != null ? subscriptionId.hashCode() : 0;
    }

    public SubscriptionId getSubscriptionId() {
        return subscriptionId;
    }
}