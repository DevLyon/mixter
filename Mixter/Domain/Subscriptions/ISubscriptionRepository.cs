using System.Collections.Generic;

namespace Mixter.Domain.Subscriptions
{
    public interface ISubscriptionRepository
    {
        IEnumerable<UserId> GetFollowers(UserId userId);
    }
}