using System.Collections.Generic;
using Mixter.Domain.Identity;

namespace Mixter.Domain.Core.Subscriptions
{
    public interface IFollowersRepository
    {
        void Save(FollowerProjection projection);
        void Remove(FollowerProjection projection);
        IEnumerable<UserId> GetFollowers(UserId followee);
    }
}