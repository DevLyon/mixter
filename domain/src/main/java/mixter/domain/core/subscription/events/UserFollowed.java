package mixter.domain.core.subscription.events;

import mixter.domain.AggregateId;
import mixter.domain.Event;
import mixter.domain.core.subscription.SubscriptionId;

public class UserFollowed implements Event {
    private SubscriptionId subscriptionId;

    public UserFollowed(SubscriptionId subscriptionId) {

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

        UserFollowed that = (UserFollowed) o;

        return !(subscriptionId != null ? !subscriptionId.equals(that.subscriptionId) : that.subscriptionId != null);

    }

    @Override
    public int hashCode() {
        return subscriptionId != null ? subscriptionId.hashCode() : 0;
    }
}
