using System.Collections.Generic;
using Mixter.Domain.Identity;

namespace Mixter.Domain.Core.Subscriptions
{
    public interface ISubscriptionsRepository
    {
        Subscription GetSubscription(SubscriptionId id);
        IEnumerable<Subscription> GetSubscriptionsOfUser(UserId userId);
    }
}