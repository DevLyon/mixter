using System.Collections.Generic;

namespace Mixter.Domain.Subscriptions
{
    public interface ISubscriptionRepository
    {
        IEnumerable<Subscription> GetFollowers(UserId userId);
    }
}