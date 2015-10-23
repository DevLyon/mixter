package mixter.domain.core.subscription;

import mixter.doc.Repository;
import mixter.domain.identity.UserId;

import java.util.Iterator;

@Repository
public interface SubscriptionRepository {
    Subscription getById(SubscriptionId subscriptionId);

    Iterator<Subscription> getSubscriptionsOfUser(UserId userId);
}

