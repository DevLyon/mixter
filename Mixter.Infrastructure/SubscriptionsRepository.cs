using System.Collections.Generic;
using System.Linq;
using Mixter.Domain.Core.Subscriptions;
using Mixter.Domain.Identity;

namespace Mixter.Infrastructure
{
    public class SubscriptionsRepository : ISubscriptionsRepository
    {
        private readonly EventsStore _eventsStore;
        private readonly FollowersRepository _followersRepository;

        public SubscriptionsRepository(EventsStore eventsStore, FollowersRepository followersRepository)
        {
            _eventsStore = eventsStore;
            _followersRepository = followersRepository;
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

        public IEnumerable<Subscription> GetSubscriptionsOfUser(UserId userId)
        {
            return _followersRepository.GetFollowers(userId)
                                        .Select(follower => new SubscriptionId(follower, userId))
                                        .Select(GetSubscription);
        }
    }
}
