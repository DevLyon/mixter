package mixter.domain.core.subscription.handlers;

import mixter.domain.core.subscription.FakeFollowerRepository;
import mixter.domain.core.subscription.FollowerRepository;
import mixter.domain.core.subscription.SubscriptionId;
import mixter.domain.core.subscription.events.UserFollowed;
import mixter.domain.core.subscription.events.UserUnfollowed;
import mixter.domain.identity.UserId;
import org.junit.Before;
import org.junit.Test;

import static org.assertj.core.api.Assertions.assertThat;

public class UpdateFollowersTest {

    public static final UserId FOLLOWEE = new UserId("followee@mix-it.fr");
    public static final UserId FOLLOWER = new UserId("follower@mix-it.fr");

    private FollowerRepository repository;

    @Before
    public void setUp() throws Exception {
        repository = new FakeFollowerRepository();
    }

    @Test
    public void shouldAddFollowerOfFolloweeOnUserFollowed() {
        // Given
        UserFollowed userFollowed = new UserFollowed(new SubscriptionId(FOLLOWER, FOLLOWEE));
        UpdateFollowers handler = new UpdateFollowers(repository);
        // When
        handler.apply(userFollowed);
        // Then
        assertThat(repository.getFollowers(FOLLOWEE)).containsExactly(FOLLOWER);
    }

    @Test
    public void shouldRemoveFollowerOfFolloweeOnUserUnfollowed() {
        // Given
        UserFollowed userFollowed = new UserFollowed(new SubscriptionId(FOLLOWER, FOLLOWEE));
        UserUnfollowed userUnfollowed = new UserUnfollowed(new SubscriptionId(FOLLOWER, FOLLOWEE));
        UpdateFollowers handler = new UpdateFollowers(repository);
        handler.apply(userFollowed);
        // When
        handler.apply(userUnfollowed);
        // Then
        assertThat(repository.getFollowers(FOLLOWEE)).isEmpty();
    }
}
