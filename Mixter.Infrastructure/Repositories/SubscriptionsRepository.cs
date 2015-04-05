using Mixter.Domain.Core.Messages.Handlers;
using Mixter.Domain.Core.Subscriptions;

namespace Mixter.Infrastructure.Repositories
{
    public class SubscriptionsesRepository : ISubscriptionsRepository
    {
        private readonly EventsStore _eventsStore;

        public SubscriptionsesRepository(EventsStore eventsStore)
        {
            _eventsStore = eventsStore;
        }

        public Subscription Get(SubscriptionId subscriptionId)
        {
            return new Subscription(_eventsStore.GetEventsOfAggregate(subscriptionId));
        }
    }
}