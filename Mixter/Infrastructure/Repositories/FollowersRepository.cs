using System.Collections.Generic;
using System.Linq;
using Mixter.Domain.Core.Subscriptions;
using Mixter.Domain.Identity;

namespace Mixter.Infrastructure.Repositories
{
    public class FollowersRepository : IFollowersRepository
    {
        private readonly HashSet<FollowerProjection> _projections = new HashSet<FollowerProjection>();

        public void Save(FollowerProjection projection)
        {
            _projections.Add(projection);
        }

        public void Remove(FollowerProjection projection)
        {
            _projections.Remove(projection);
        }

        public IEnumerable<UserId> GetFollowers(UserId userId)
        {
            return _projections.Where(o => o.Followee.Equals(userId)).Select(o => o.Follower);
        } 
    }
}