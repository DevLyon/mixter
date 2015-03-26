using System.Collections.Generic;
using System.Linq;
using Mixter.Domain;
using Mixter.Domain.Subscriptions;
using Mixter.Domain.Subscriptions.Events;

namespace Mixter.Infrastructure
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly EventsDatabase _database;

        public SubscriptionRepository(EventsDatabase database)
        {
            _database = database;
        }

        public IEnumerable<Subscription> GetFollowers(UserId userId)
        {
            return _database.GetEvents()
                            .OfType<UserFollowed>()
                            .Where(evt => IsFollowee(userId, evt))
                            .Select(evt => evt.SubscriptionId)
                            .Select(CreateAggregate);
        }

        private static bool IsFollowee(UserId userId, UserFollowed evt)
        {
            return evt.SubscriptionId.Followee.Equals(userId);
        }

        private Subscription CreateAggregate(SubscriptionId id)
        {
            return new Subscription(_database.GetEventsOfAggregate(id));
        }
    }
}