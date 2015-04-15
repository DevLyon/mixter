package mixter.domain.core.subscription;

import mixter.domain.DomainTest;
import mixter.domain.SpyEventPublisher;
import mixter.domain.core.subscription.events.UserFollowed;
import mixter.domain.identity.UserId;
import org.junit.Before;
import org.junit.Test;

import static org.assertj.core.api.Assertions.assertThat;

public class SubscriptionTest extends DomainTest {

    private SpyEventPublisher eventPublisher;
    public static final UserId FOLLOWER = new UserId("follower@mix-it.fr");
    public static final UserId FOLLOWEE = new UserId("followee@mix-it.fr");

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

}
