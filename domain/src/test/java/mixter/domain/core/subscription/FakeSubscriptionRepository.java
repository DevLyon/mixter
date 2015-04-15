package mixter.domain.core.subscription;

import org.assertj.core.util.Sets;

import java.util.Set;

public class FakeSubscriptionRepository implements SubscriptionRepository {
    private Set<Subscription> subscriptions = Sets.newHashSet();

    public void add(Subscription subscription) {
        subscriptions.add(subscription);
    }

    @Override
    public Subscription getById(SubscriptionId subscriptionId) {
        return subscriptions.stream().filter(subscription -> subscription.getId().equals(subscriptionId)).findFirst().get();
    }
}
