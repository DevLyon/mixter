package mixter;

import org.junit.Test;

import static org.assertj.core.api.Assertions.assertThat;

public class SubscriptionTest extends AggregateTest {
    @Test
    public void WhenAUserFollowsAnotherUserFollowedIsRaised() throws Exception {
        //Given
        UserId follower = new UserId();
        UserId followee = new UserId();

        SpyEventPublisher eventPublisher = new SpyEventPublisher();
        //When
        Subscription.follow(follower, followee, eventPublisher);

        //Then
        UserFollowed userFollowed = new UserFollowed(new SubscriptionId(follower, followee));
        assertThat(eventPublisher.publishedEvents).containsExactly(userFollowed);
    }

}
