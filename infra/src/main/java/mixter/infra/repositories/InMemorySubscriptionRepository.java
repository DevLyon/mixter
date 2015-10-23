package mixter.infra.repositories;

import mixter.domain.Event;
import mixter.domain.core.subscription.FollowerRepository;
import mixter.domain.core.subscription.Subscription;
import mixter.domain.core.subscription.SubscriptionId;
import mixter.domain.core.subscription.SubscriptionRepository;
import mixter.domain.identity.UserId;
import mixter.infra.EventStore;

import java.util.Iterator;
import java.util.List;

public class InMemorySubscriptionRepository extends AggregateRepository<Subscription> implements SubscriptionRepository {

    private final FollowerRepository followersRepository;

    public InMemorySubscriptionRepository(EventStore eventStore, FollowerRepository followersRepository) {
        super(eventStore);
        this.followersRepository = followersRepository;
    }

    @Override
    protected Subscription fromHistory(List<Event> history) {
        return new Subscription(history);
    }

    @Override
    public Subscription getById(SubscriptionId subscriptionId) {
        return super.getById(subscriptionId);
    }

    @Override
    public Iterator<Subscription> getSubscriptionsOfUser(UserId userId) {
        return followersRepository.getFollowers(userId).stream()
                .map(follower->new SubscriptionId(follower, userId))
                .map(this::getById).iterator();
    }
}
