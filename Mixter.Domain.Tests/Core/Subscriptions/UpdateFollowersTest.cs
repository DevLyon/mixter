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
            var follower = new UserId("follower@mixit.fr");
            var followee = new UserId("followee@mixit.fr");

            _handler.Handle(new UserFollowed(new SubscriptionId(follower, followee)));

            Check.That(_followersRepository.GetFollowers(followee)).ContainsExactly(follower);
        }
    }
}
