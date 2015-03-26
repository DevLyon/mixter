using System.Collections.Generic;

namespace Mixter.Domain.Core.Subscriptions
{
    public interface ISubscriptionRepository
    {
        IEnumerable<Subscription> GetFollowers(UserId userId);
    }
}