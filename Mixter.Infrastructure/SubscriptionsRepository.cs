using Mixter.Domain.Core.Subscriptions;

namespace Mixter.Infrastructure
{
    public class SubscriptionsRepository : ISubscriptionsRepository
    {
        private readonly EventsStore _eventsStore;

        public SubscriptionsRepository(EventsStore eventsStore)
        {
            _eventsStore = eventsStore;
        }

        public Subscription Get(SubscriptionId subscriptionId)
        {
            return new Subscription(_eventsStore.GetEventsOfAggregate(subscriptionId));
        }
    }
}