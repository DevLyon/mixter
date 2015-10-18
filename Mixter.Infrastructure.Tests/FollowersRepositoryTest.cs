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

        [Fact]
        public void WhenSaveSeveralFollowersButNotSameFolloweeThenGetFollowersReturnOnlyFollowerIdsOfFollowee()
        {
            var followee1 = new UserId("followee1@mixit.fr");
            var followee2 = new UserId("followee2@mixit.fr");
            var follower1 = new UserId("follower1@mixit.fr");
            var follower2 = new UserId("follower2@mixit.fr");
            _repository.Save(new FollowerProjection(followee1, follower1));
            _repository.Save(new FollowerProjection(followee2, follower2));

            var followers = _repository.GetFollowers(followee1);

            Check.That(followers).ContainsExactly(follower1);
        }

        [Fact]
        public void WhenRemoveFollowerThenGetFollowersReturnEmpty()
        {
            var followee = new UserId("followee@mixit.fr");
            var follower = new UserId("follower@mixit.fr");
            _repository.Save(new FollowerProjection(followee, follower));
            _repository.Remove(new FollowerProjection(followee, follower));

            var followers = _repository.GetFollowers(followee);

            Check.That(followers).IsEmpty();
        }
    }
}
