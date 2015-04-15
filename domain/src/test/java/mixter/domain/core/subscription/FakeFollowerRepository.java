package mixter.domain.core.subscription;

import mixter.domain.identity.UserId;
import org.assertj.core.util.Maps;
import org.assertj.core.util.Sets;

import java.util.Map;
import java.util.Set;

public class FakeFollowerRepository implements FollowerRepository {
    Map<UserId, Set<UserId>> followers = Maps.newHashMap();

    @Override
    public Set<UserId> getFollowers(UserId followee) {
        return this.followers.getOrDefault(followee, Sets.newHashSet());
    }

    @Override
    public void saveFollower(UserId followee, UserId follower) {
        Set<UserId> userFollowers = followers.getOrDefault(followee, Sets.newHashSet());
        userFollowers.add(follower);
        followers.put(followee, userFollowers);
    }

    @Override
    public void removeFollower(UserId followee, UserId follower) {
        Set<UserId> userFollowers = followers.getOrDefault(followee, Sets.newHashSet());
        userFollowers.remove(follower);
        followers.put(followee, userFollowers);
    }
}
