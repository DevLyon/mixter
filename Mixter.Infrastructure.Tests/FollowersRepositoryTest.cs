using Mixter.Domain.Core.Subscriptions;
using Mixter.Domain.Identity;
using NFluent;
using Xunit;

namespace Mixter.Infrastructure.Tests
{
    public class FollowersRepositoryTest
    {
        private FollowersRepository _repository;
        
        public FollowersRepositoryTest()
        {
            _repository = new FollowersRepository();
        }

        [Fact]
        public void WhenSaveThenGetFollowersReturnFollowerId()
        {
            var followee = new UserId("followee@mixit.fr");
            var follower = new UserId("follower@mixit.fr");
            _repository.Save(new FollowerProjection(followee, follower));

            var followers = _repository.GetFollowers(followee);

            Check.That(followers).ContainsExactly(follower);
        }
    }
}
