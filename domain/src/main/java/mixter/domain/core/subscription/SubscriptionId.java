package mixter.domain.core.subscription;

import mixter.domain.AggregateId;
import mixter.domain.identity.UserId;

public class SubscriptionId implements AggregateId {
    private final UserId follower;
    private final UserId followee;

    public SubscriptionId(UserId follower, UserId followee) {
        this.follower = follower;
        this.followee = followee;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;

        SubscriptionId that = (SubscriptionId) o;

        if (follower != null ? !follower.equals(that.follower) : that.follower != null) return false;
        return !(followee != null ? !followee.equals(that.followee) : that.followee != null);

    }

    @Override
    public int hashCode() {
        int result = follower != null ? follower.hashCode() : 0;
        result = 31 * result + (followee != null ? followee.hashCode() : 0);
        return result;
    }

    public UserId getFollowee() {
        return followee;
    }

    public UserId getFollower() {
        return follower;
    }
}
