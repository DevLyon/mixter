using Mixter.Domain.Core.Messages.Handlers;
using Mixter.Domain.Core.Subscriptions;

namespace Mixter.Infrastructure.Repositories
{
    public class SubscriptionsesRepository : ISubscriptionsRepository
    {
        private readonly EventsDatabase _eventsDatabase;

        public SubscriptionsesRepository(EventsDatabase eventsDatabase)
        {
            _eventsDatabase = eventsDatabase;
        }

        public Subscription Get(SubscriptionId subscriptionId)
        {
            return new Subscription(_eventsDatabase.GetEventsOfAggregate(subscriptionId));
        }
    }
}