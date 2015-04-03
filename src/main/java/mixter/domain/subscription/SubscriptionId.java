package mixter.domain.subscription;

import mixter.UserId;

class SubscriptionId {
    private final UserId follower;
    private final UserId followee;

    public SubscriptionId(UserId follower, UserId followee) {
        this.follower = follower;
        this.followee = followee;
    }

    public UserId getFollower() {
        return follower;
    }

    public UserId getFollowee() {
        return followee;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;

        SubscriptionId that = (SubscriptionId) o;

        return !(follower != null ? !follower.equals(that.follower) : that.follower != null) && !(followee != null ? !followee.equals(that.followee) : that.followee != null);

    }

    @Override
    public int hashCode() {
        int result = follower != null ? follower.hashCode() : 0;
        result = 31 * result + (followee != null ? followee.hashCode() : 0);
        return result;
    }
}
