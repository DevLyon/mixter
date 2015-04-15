package mixter.domain.core.subscription.handlers;

import mixter.domain.DomainTest;
import mixter.domain.SpyEventPublisher;
import mixter.domain.core.message.MessageId;
import mixter.domain.core.message.events.MessageQuacked;
import mixter.domain.core.subscription.FakeFollowerRepository;
import mixter.domain.core.subscription.FakeSubscriptionRepository;
import mixter.domain.core.subscription.SubscriptionId;
import mixter.domain.core.subscription.events.FolloweeMessageQuacked;
import mixter.domain.core.subscription.events.UserFollowed;
import mixter.domain.identity.UserId;
import org.junit.Before;
import org.junit.Test;

import static org.assertj.core.api.Assertions.assertThat;

public class NotifyFollowerOfFolloweeMessageTest extends DomainTest {

    public String CONTENT = "Content";
    public UserId AUTHOR_ID = new UserId("author@mix-it.fr");
    public UserId FOLLOWER_ID = new UserId("follower@mix-it.fr");
    public MessageId MESSAGE_ID = MessageId.generate();

    private SpyEventPublisher eventPublisher;
    private FakeSubscriptionRepository subscriptionRepository;
    private FakeFollowerRepository followerRepository;

    @Before
    public void setUp() throws Exception {
        eventPublisher = new SpyEventPublisher();
        subscriptionRepository = new FakeSubscriptionRepository();
        followerRepository = new FakeFollowerRepository();
    }

    @Test
    public void APublishedMessageShouldNotifyFollowers() {
        // Given
        MessageQuacked messageQuacked = new MessageQuacked(MESSAGE_ID, CONTENT, AUTHOR_ID);
        subscriptionRepository.add(subscriptionFor(new UserFollowed(new SubscriptionId(FOLLOWER_ID, AUTHOR_ID))));
        followerRepository.saveFollower(AUTHOR_ID, FOLLOWER_ID);

        NotifyFollowerOfFolloweeMessage handler = new NotifyFollowerOfFolloweeMessage(followerRepository, subscriptionRepository, eventPublisher);

        // When
        handler.apply(messageQuacked);
        // Then
        FolloweeMessageQuacked followeeMessagePublished = new FolloweeMessageQuacked(new SubscriptionId(FOLLOWER_ID, AUTHOR_ID), MESSAGE_ID);
        assertThat(eventPublisher.publishedEvents).containsExactly(followeeMessagePublished);
    }
}
