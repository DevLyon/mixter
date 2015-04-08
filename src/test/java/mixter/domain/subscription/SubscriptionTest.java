package mixter.domain.subscription;

import mixter.AggregateTest;
import mixter.Event;
import mixter.UserId;
import mixter.domain.subscription.events.UserFollowed;
import mixter.domain.subscription.events.UserUnfollowed;
import org.junit.Test;

import java.util.List;

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

    @Test
    public void WhenAUserUnfollowsAnotherUserUnfollowedIsRaised() throws Exception {
        //Given
        UserId follower = new UserId();
        UserId followee = new UserId();
        SubscriptionId subscriptionId = new SubscriptionId(follower, followee);
        List<Event> events = history(new UserFollowed(subscriptionId));
        SpyEventPublisher eventPublisher = new SpyEventPublisher();
        Subscription subscription = new Subscription(events);
        //When
        subscription.unfollow(eventPublisher);

        //Then
        UserUnfollowed userUnfollowed = new UserUnfollowed(subscriptionId);
        assertThat(eventPublisher.publishedEvents).containsExactly(userUnfollowed);
    }
}
