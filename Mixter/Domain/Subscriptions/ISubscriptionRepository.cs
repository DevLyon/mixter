using System.Collections.Generic;

namespace Mixter.Domain.Subscriptions
{
    public interface ISubscriptionRepository
    {
        //todo: on ne doit pas retournée des userId dans le subscriptionRepo!!!!!!!!
        IEnumerable<UserId> GetFollowers(UserId userId);
    }
}