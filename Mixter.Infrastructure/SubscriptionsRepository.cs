using System.Linq;
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
            var events = _eventsStore.GetEventsOfAggregate(id).ToArray();
            if (!events.Any())
            {
                throw new UnknownSubscription(id);
            }

            return new Subscription(events);
        }
    }
}
