using Mixter.Domain.Core.Subscriptions;
using Mixter.Domain.Core.Subscriptions.Events;
using Mixter.Domain.Core.Subscriptions.Handlers;
using Mixter.Domain.Identity;
using Mixter.Infrastructure;
using NFluent;
using Xunit;

namespace Mixter.Domain.Tests.Core.Subscriptions
{
    public class UpdateFollowersTest
    {
        private static readonly UserId Follower = new UserId("follower@mixit.fr");
        private static readonly UserId Followee = new UserId("followee@mixit.fr");

        private readonly FollowersRepository _followersRepository;
        private readonly UpdateFollowers _handler;

        public UpdateFollowersTest()
        {
            _followersRepository = new FollowersRepository();
            _handler = new UpdateFollowers(_followersRepository);
        }

        [Fact]
        public void WhenUserFollowedThenSaveFollower()
        {
            _handler.Handle(new UserFollowed(new SubscriptionId(Follower, Followee)));

            Check.That(_followersRepository.GetFollowers(Followee)).ContainsExactly(Follower);
        }

        [Fact]
        public void WhenUserUnfollowedThenRemoveFollower()
        {
            _handler.Handle(new UserFollowed(new SubscriptionId(Follower, Followee)));

            _handler.Handle(new UserUnfollowed(new SubscriptionId(Follower, Followee)));

            Check.That(_followersRepository.GetFollowers(Followee)).IsEmpty();
        }
    }
}
