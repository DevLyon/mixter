package mixter.infra.repositories;

import mixter.domain.EventPublisher;
import mixter.domain.core.subscription.FollowerRepository;
import mixter.domain.core.subscription.Subscription;
import mixter.domain.core.subscription.SubscriptionId;
import mixter.domain.core.subscription.SubscriptionRepository;
import mixter.domain.core.subscription.events.UserFollowed;
import mixter.domain.core.subscription.events.UserUnfollowed;
import mixter.domain.identity.UserId;
import mixter.infra.EventStore;
import mixter.infra.InMemoryEventStore;
import mixter.infra.SpyEventPublisher;
import org.junit.Before;
import org.junit.Rule;
import org.junit.Test;
import org.junit.rules.ExpectedException;

import java.util.Iterator;
import java.util.NoSuchElementException;

import static org.assertj.core.api.Assertions.assertThat;

public class InMemorySubscriptionRepositoryTest {
    public static final UserId ALICE_ID= new UserId("alice@mix-it.fr");
    public static final UserId BOB_ID= new UserId("bob@mix-it.fr");
    private static final SubscriptionId SUBSCRIPTION_ID = new SubscriptionId(ALICE_ID, BOB_ID);
    private EventStore eventStore;
    private SubscriptionRepository subscriptionRepository;
    private FollowerRepository followersRepository;


    @Before
    public void setUp() throws Exception {
        eventStore = new InMemoryEventStore();
        followersRepository=new InMemoryFollowerRepository();
        subscriptionRepository=new InMemorySubscriptionRepository(eventStore, followersRepository);
    }

    @Rule
    public ExpectedException thrown = ExpectedException.none();

    @Test
    public void givenUserFollowedWhenGetSubscriptionThenReturnSubscriptionAggregate() throws Exception {
        eventStore.store(new UserFollowed(SUBSCRIPTION_ID));
        SpyEventPublisher eventPublisher= new SpyEventPublisher();

        Subscription byId = subscriptionRepository.getById(SUBSCRIPTION_ID);

        byId.unfollow(eventPublisher);

        assertThat(eventPublisher.publishedEvents).containsExactly(new UserUnfollowed(SUBSCRIPTION_ID));
    }

    @Test
    public void givenNoEventsWhenGetSubscriptionThenThrowUnknownSubscriptionException() throws Exception {
        thrown.expect(NoSuchElementException.class);

        Subscription byId = subscriptionRepository.getById(SUBSCRIPTION_ID);
    }

    @Test
    public void givenAFolloweeWithTwoFollowersWhenGetSubscriptionsOfUserThenReturnAllSubscriptionAggregatesOfUser() throws Exception {
        UserId followee = new UserId("followee@mix-it.fr");
        UserId follower1 = new UserId("follower1@mix-it.fr");
        UserId follower2 = new UserId("follower2@mix-it.fr");

        followersRepository.saveFollower(followee, follower1);
        followersRepository.saveFollower(followee, follower2);
        eventStore.store(new UserFollowed(new SubscriptionId(follower1,followee)));
        eventStore.store(new UserFollowed(new SubscriptionId(follower2,followee)));

        Iterator<Subscription> subscriptions = subscriptionRepository.getSubscriptionsOfUser(followee);

        assertThat(subscriptions).hasSize(2);
    }
}