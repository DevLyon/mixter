using Mixter.Domain.Core.Subscriptions;

namespace Mixter.Infrastructure
{
    public class SubscriptionsRepository
    {
        private readonly EventsStore _eventsStore;

        public SubscriptionsRepository(EventsStore eventsStore)
        {
            _eventsStore = eventsStore;
        }

        public Subscription GetSubscription(SubscriptionId id)
        {
            return new Subscription(_eventsStore.GetEventsOfAggregate(id));
        }
    }
}
