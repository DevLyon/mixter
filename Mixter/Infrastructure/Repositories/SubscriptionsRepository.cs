using System.Collections.Generic;
using System.Linq;
using Mixter.Domain;
using Mixter.Domain.Core;
using Mixter.Domain.Core.Subscriptions;
using Mixter.Domain.Core.Subscriptions.Events;
using Mixter.Domain.Identity;

namespace Mixter.Infrastructure.Repositories
{
    public class SubscriptionsRepository : ISubscriptionRepository
    {
        private readonly EventsDatabase _database;

        public SubscriptionsRepository(EventsDatabase database)
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