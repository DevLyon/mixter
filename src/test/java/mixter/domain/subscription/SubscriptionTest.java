package mixter.domain.subscription;

import mixter.AggregateTest;
import mixter.Event;
import mixter.UserId;
import mixter.domain.subscription.events.UserFollowed;
import mixter.domain.subscription.events.UserUnfollowed;
import org.junit.Before;
import org.junit.Test;

import static org.assertj.core.api.Assertions.assertThat;

public class SubscriptionTest extends AggregateTest {

    private SpyEventPublisher eventPublisher;
    public static final UserId FOLLOWER = new UserId();
    public static final UserId FOLLOWEE = new UserId();

    @Before
    public void setUp() throws Exception {
        eventPublisher = new SpyEventPublisher();
    }

    @Test
    public void WhenAUserFollowsAnotherUserFollowedIsRaised() throws Exception {
        //Given

        //When
        Subscription.follow(FOLLOWER, FOLLOWEE, eventPublisher);

        //Then
        UserFollowed userFollowed = new UserFollowed(new SubscriptionId(FOLLOWER, FOLLOWEE));
        assertThat(eventPublisher.publishedEvents).containsExactly(userFollowed);
    }

    @Test
    public void WhenAUserUnfollowsAnotherUserUnfollowedIsRaised() throws Exception {
        //Given
        SubscriptionId subscriptionId = new SubscriptionId(FOLLOWER, FOLLOWEE);
        Subscription subscription = subscriptionFor(
                new UserFollowed(subscriptionId)
        );
        //When
        subscription.unfollow(eventPublisher);

        //Then
        UserUnfollowed userUnfollowed = new UserUnfollowed(subscriptionId);
        assertThat(eventPublisher.publishedEvents).containsExactly(userUnfollowed);
    }

    Subscription subscriptionFor(Event... events) {
        return new Subscription(history(events));
    }
}
