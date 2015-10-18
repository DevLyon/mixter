using System.Collections.Generic;
using Mixter.Domain.Identity;

namespace Mixter.Domain.Core.Subscriptions
{
    [Repository]
    public interface IFollowersRepository
    {
        void Save(FollowerProjection projection);

        void Remove(FollowerProjection projection);

        [Query]
        IEnumerable<UserId> GetFollowers(UserId followee);
    }
}