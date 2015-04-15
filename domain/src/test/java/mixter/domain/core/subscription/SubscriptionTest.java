package mixter.domain.core.subscription;

import mixter.domain.DomainTest;
import mixter.domain.Event;
import mixter.domain.SpyEventPublisher;
import mixter.domain.core.message.MessageId;
import mixter.domain.core.subscription.events.FolloweeMessageQuacked;
import mixter.domain.core.subscription.events.UserFollowed;
import mixter.domain.core.subscription.events.UserUnfollowed;
import mixter.domain.identity.UserId;
import org.junit.Before;
import org.junit.Test;

import static org.assertj.core.api.Assertions.assertThat;

public class SubscriptionTest extends DomainTest {

    private SpyEventPublisher eventPublisher;
    public UserId FOLLOWER = new UserId("follower@mix-it.fr");
    public UserId FOLLOWEE = new UserId("followee@mix-it.fr");
    public SubscriptionId SUBSCRIPTION_ID = new SubscriptionId(FOLLOWER, FOLLOWEE);

    @Before
    public void setUp() throws Exception {
        eventPublisher = new SpyEventPublisher();
    }

    @Test
    public void whenAUserFollowsAnotherUserFollowedIsRaised() throws Exception {
        //Given

        //When
        Subscription.follow(FOLLOWER, FOLLOWEE, eventPublisher);

        //Then
        UserFollowed userFollowed = new UserFollowed(new SubscriptionId(FOLLOWER, FOLLOWEE));
        assertThat(eventPublisher.publishedEvents).containsExactly(userFollowed);
    }

    @Test
    public void whenAUserUnfollowsAnotherUserUnfollowedIsRaised() throws Exception {
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
    public void whenNotifyFollowerThenFolloweeMessageQuackedIsRaised() {
        //Given
        Subscription subscription = subscriptionFor(
                new UserFollowed(SUBSCRIPTION_ID)
        );
        MessageId messageId = MessageId.generate();

        //When
        subscription.notifyFollower(messageId, eventPublisher);

        assertThat(eventPublisher.publishedEvents).containsExactly(new FolloweeMessageQuacked(SUBSCRIPTION_ID, messageId));
    }


    protected Subscription subscriptionFor(Event... events) {
        return new Subscription(history(events));
    }
}
