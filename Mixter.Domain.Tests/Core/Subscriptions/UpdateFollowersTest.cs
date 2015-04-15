using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mixter.Domain.Core.Subscriptions;
using Mixter.Domain.Core.Subscriptions.Events;
using Mixter.Domain.Core.Subscriptions.Handlers;
using Mixter.Domain.Identity;
using Mixter.Infrastructure;
using NFluent;

namespace Mixter.Domain.Tests.Core.Subscriptions
{
    [TestClass]
    public class UpdateFollowersTest
    {
        private FollowersRepository _followersRepository;
        private UpdateFollowers _handler;

        [TestInitialize]
        public void Initialize()
        {
            _followersRepository = new FollowersRepository();
            _handler = new UpdateFollowers(_followersRepository);
        }

        [TestMethod]
        public void WhenUserFollowedThenSaveFollower()
        {
            var follower = new UserId("follower@mixit.fr");
            var followee = new UserId("followee@mixit.fr");

            _handler.Handle(new UserFollowed(new SubscriptionId(follower, followee)));

            Check.That(_followersRepository.GetFollowers(followee)).ContainsExactly(follower);
        }

        [TestMethod]
        public void WhenUserUnfollowedThenRemoveFollower()
        {
            var follower = new UserId("follower@mixit.fr");
            var followee = new UserId("followee@mixit.fr");
            _handler.Handle(new UserFollowed(new SubscriptionId(follower, followee)));

            _handler.Handle(new UserUnfollowed(new SubscriptionId(follower, followee)));

            Check.That(_followersRepository.GetFollowers(followee)).IsEmpty();
        }
    }
}
