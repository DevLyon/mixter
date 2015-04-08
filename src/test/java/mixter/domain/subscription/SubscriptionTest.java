package mixter.domain.subscription;

import mixter.AggregateTest;
import mixter.Event;
import mixter.UserId;
import mixter.domain.message.MessageId;
import mixter.domain.subscription.events.FolloweeMessagePublished;
import mixter.domain.subscription.events.UserFollowed;
import mixter.domain.subscription.events.UserUnfollowed;
import org.junit.Before;
import org.junit.Test;

import static org.assertj.core.api.Assertions.assertThat;

public class SubscriptionTest extends AggregateTest {

    private SpyEventPublisher eventPublisher;
    public static final UserId FOLLOWER = new UserId();
    public static final UserId FOLLOWEE = new UserId();
    public static final SubscriptionId SUBSCRIPTION_ID = new SubscriptionId(FOLLOWER, FOLLOWEE);

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
        Subscription subscription = subscriptionFor(
                new UserFollowed(SUBSCRIPTION_ID)
        );
        //When
        subscription.unfollow(eventPublisher);

        //Then
        UserUnfollowed userUnfollowed = new UserUnfollowed(SUBSCRIPTION_ID);
        assertThat(eventPublisher.publishedEvents).containsExactly(userUnfollowed);
    }

    @Test
    public void WhenNotifyFollowerThenFollowerMessagePublishedIsRaised() {
        //Given
        Subscription subscription = subscriptionFor(
                new UserFollowed(SUBSCRIPTION_ID)
        );
        MessageId messageId = new MessageId();

        //When
        subscription.notifyFollower(messageId, eventPublisher);

        assertThat(eventPublisher.publishedEvents).containsExactly(new FolloweeMessagePublished(SUBSCRIPTION_ID, messageId));
    }

    @Test
    public void GivenUnfollowWhenNotifyFollowerThenDoNotRaisedFollowerMessagePublished() {
        // Given
        Subscription subscription = subscriptionFor(
                new UserFollowed(SUBSCRIPTION_ID),
                new UserUnfollowed(SUBSCRIPTION_ID)
        );
        MessageId messageId = new MessageId();

        // When
        subscription.notifyFollower(messageId, eventPublisher);

        assertThat(eventPublisher.publishedEvents).isEmpty();
    }

    Subscription subscriptionFor(Event... events) {
        return new Subscription(history(events));
    }
}
