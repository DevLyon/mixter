using System;
using System.Collections.Generic;
using Mixter.Domain;
using Mixter.Domain.Subscriptions;

namespace Mixter.Infrastructure
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        public IEnumerable<UserId> GetFollowers(UserId userId)
        {
            throw new NotImplementedException();
        }
    }
}