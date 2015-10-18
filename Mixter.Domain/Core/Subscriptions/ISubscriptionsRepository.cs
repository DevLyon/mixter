using System.Collections.Generic;
using Mixter.Domain.Identity;

namespace Mixter.Domain.Core.Subscriptions
{
    [Repository]
    public interface ISubscriptionsRepository
    {
        [Query]
        Subscription GetSubscription(SubscriptionId id);

        [Query]
        IEnumerable<Subscription> GetSubscriptionsOfUser(UserId userId);
    }
}