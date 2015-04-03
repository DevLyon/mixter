package mixter;

import org.junit.Test;

import java.util.ArrayList;
import java.util.List;

import static org.assertj.core.api.Assertions.*;

public class SubscriptionTest {
    @Test
    public void WhenAUserFollowsAnotherUserFollowedIsRaised() throws Exception {
        //Given
        UserId follower = new UserId();
        UserId followee = new UserId();

        SpyEventPublisher eventPublisher = new SpyEventPublisher();
        //When
        Subscription.follow(follower, followee, eventPublisher);

        //Then
        UserFollowed userFollowed=new UserFollowed(new SubscriptionId(follower, followee));
        assertThat(eventPublisher.publishedEvents).containsExactly(userFollowed);
    }

    class SpyEventPublisher implements EventPublisher {
        public List<Event> publishedEvents = new ArrayList<>();

        public void publish(Event event) {
            publishedEvents.add(event);
        }
    }
}
