package mixter.domain.core.subscription;

import mixter.doc.Repository;

@Repository
public interface SubscriptionRepository {
    Subscription getById(SubscriptionId subscriptionId);
}

