package mixter.infra.repositories;

import mixter.domain.identity.UserId;
import org.junit.Test;

import java.util.Set;

import static org.assertj.core.api.Assertions.assertThat;

public class InMemoryFollowerRepositoryTest {

    public static final UserId FOLLOWEE = new UserId("followee@mix-it.fr");
    public static final UserId OTHER_FOLLOWEE = new UserId("other-followee@mix-it.fr");
    public static final UserId FOLLOWER = new UserId("follower@mix-it.fr");
    public static final UserId OTHER_FOLLOWER = new UserId("other-follower@mix-it.fr");

    @Test
    public void emptyRepositoryShouldHaveNoFollowersForUserId() {
        // Given
        InMemoryFollowerRepository repository = new InMemoryFollowerRepository();
        // When
        Set<UserId> followers = repository.getFollowers(FOLLOWEE);
        // Then
        assertThat(followers).isEmpty();
    }

    @Test
    public void repositoryShouldReturnASavedFollower() {
        // Given
        InMemoryFollowerRepository repository = new InMemoryFollowerRepository();
        // When
        repository.saveFollower(FOLLOWEE, FOLLOWER);
        // Then
        assertThat(repository.getFollowers(FOLLOWEE)).containsExactly(FOLLOWER);
    }

    @Test
    public void repositoryShouldReturnAUniqueSavedFollowers() {
        // Given
        InMemoryFollowerRepository repository = new InMemoryFollowerRepository();
        // When
        repository.saveFollower(FOLLOWEE, FOLLOWER);
        repository.saveFollower(FOLLOWEE, FOLLOWER);
        repository.saveFollower(FOLLOWEE, FOLLOWER);
        // Then
        assertThat(repository.getFollowers(FOLLOWEE)).containsExactly(FOLLOWER);
    }

    @Test
    public void repositoryShouldOnlyReturnFollowersOfFollowee() {
        // Given
        InMemoryFollowerRepository repository = new InMemoryFollowerRepository();
        // When
        repository.saveFollower(FOLLOWEE, FOLLOWER);
        repository.saveFollower(OTHER_FOLLOWEE, OTHER_FOLLOWER);
        // Then
        assertThat(repository.getFollowers(FOLLOWEE)).containsExactly(FOLLOWER);
    }

    @Test
    public void repositoryShouldRemoveFollowerOfFollowee() {
        // Given
        InMemoryFollowerRepository repository = new InMemoryFollowerRepository();
        // When
        repository.saveFollower(FOLLOWEE, FOLLOWER);
        repository.removeFollower(FOLLOWEE, FOLLOWER);
        // Then
        assertThat(repository.getFollowers(FOLLOWEE)).isEmpty();
    }

    @Test
    public void repositoryShouldOnlyRemoveFollowerOfFollowee() {
        // Given
        InMemoryFollowerRepository repository = new InMemoryFollowerRepository();
        // When
        repository.saveFollower(FOLLOWEE, FOLLOWER);
        repository.saveFollower(OTHER_FOLLOWEE, FOLLOWER);
        repository.removeFollower(OTHER_FOLLOWEE, FOLLOWER);
        // Then
        assertThat(repository.getFollowers(FOLLOWEE)).containsExactly(FOLLOWER);
        assertThat(repository.getFollowers(OTHER_FOLLOWEE)).isEmpty();
    }
}
