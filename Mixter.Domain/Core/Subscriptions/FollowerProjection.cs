using Mixter.Domain.Identity;

namespace Mixter.Domain.Core.Subscriptions
{
    [Projection]
    public struct FollowerProjection
    {
        public UserId Followee { get; private set; }

        public UserId Follower { get; private set; }

        public FollowerProjection(UserId followee, UserId follower)
            : this()
        {
            Followee = followee;
            Follower = follower;
        }
    }
}
