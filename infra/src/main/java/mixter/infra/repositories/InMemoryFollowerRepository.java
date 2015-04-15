package mixter.infra.repositories;

import mixter.domain.core.subscription.FollowerRepository;
import mixter.domain.identity.UserId;

import java.util.HashMap;
import java.util.HashSet;
import java.util.Map;
import java.util.Set;

public class InMemoryFollowerRepository implements FollowerRepository {
    Map<UserId, Set<UserId>> followers = new HashMap<>();

    @Override
    public Set<UserId> getFollowers(UserId followee) {
        return followers.getOrDefault(followee, emptySet());
    }

    private Set<UserId> emptySet() {
        return new HashSet<>();
    }

    @Override
    public void saveFollower(UserId followee, UserId follower) {
        Set<UserId> userFollowers = this.followers.getOrDefault(followee, emptySet());
        userFollowers.add(follower);
        this.followers.put(followee, userFollowers);
    }

    @Override
    public void removeFollower(UserId followee, UserId follower) {
        Set<UserId> userFollowers = this.followers.getOrDefault(followee, emptySet());
        userFollowers.remove(follower);
        this.followers.put(followee, userFollowers);
    }
}
