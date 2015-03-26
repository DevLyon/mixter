using System;
using System.Collections.Generic;
using Mixter.Domain;
using Mixter.Domain.Subscriptions;

namespace Mixter.Infrastructure
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        public IEnumerable<Subscription> GetFollowers(UserId userId)
        {
            throw new NotImplementedException();
        }
    }
}