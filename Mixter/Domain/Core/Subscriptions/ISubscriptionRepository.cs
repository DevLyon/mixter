using System.Collections.Generic;
using Mixter.Domain.Identity;

namespace Mixter.Domain.Core.Subscriptions
{
    public interface ISubscriptionRepository
    {
        IEnumerable<Subscription> GetFollowers(UserId userId);
    }
}