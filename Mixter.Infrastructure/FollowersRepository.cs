using System;
using System.Collections.Generic;
using System.Linq;
using Mixter.Domain.Core.Subscriptions;
using Mixter.Domain.Identity;

namespace Mixter.Infrastructure
{
    public class FollowersRepository
    {
        private readonly IList<FollowerProjection> _projections = new List<FollowerProjection>();

        public void Save(FollowerProjection projection)
        {
            _projections.Add(projection);
        }

        public IEnumerable<UserId> GetFollowers(UserId followee)
        {
            return _projections.Where(o => o.Followee.Equals(followee)).Select(o => o.Follower);
        }
    }
}
