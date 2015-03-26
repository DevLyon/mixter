using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mixter.Domain;
using Mixter.Domain.Subscriptions;
using Mixter.Infrastructure;
using NFluent;

namespace Mixter.Tests.Infrastructure
{
    [TestClass]
    public class SubscriptionRepositoryTest
    {
        private static readonly UserId Followee1 = new UserId("followee1@mixit.fr");
        private static readonly UserId Followee2 = new UserId("followee2@mixit.fr");

        private static readonly UserId Follower1 = new UserId("follower1@mixit.fr");
        private static readonly UserId Follower2 = new UserId("follower2@mixit.fr");

        private EventsDatabase _database;
        private SubscriptionRepository _repository;

        [TestInitialize]
        public void Initialize()
        {
            _database = new EventsDatabase();
            _repository = new SubscriptionRepository(_database);
        }

        [TestMethod]
        public void GivenSubscriptionEventsWhenGetFollowersThenReturnSubscription()
        {
            var followee = new UserId("followee@mixit.fr");
            _database.Store(new UserFollowed(new SubscriptionId(Follower1, followee)));
            _database.Store(new UserFollowed(new SubscriptionId(Follower2, followee)));

            var subscriptions = _repository.GetFollowers(followee);

            Check.That(subscriptions).HasSize(2);
        }

        [TestMethod]
        public void WhenGetFollowersOfUserThenReturnSubscriptionOfOnlyThisUser()
        {
            _database.Store(new UserFollowed(new SubscriptionId(Follower1, Followee1)));
            _database.Store(new UserFollowed(new SubscriptionId(Follower2, Followee2)));

            var subscriptions = _repository.GetFollowers(Followee1);

            Check.That(subscriptions.Select(o => o.GetId().Follower)).HasSize(1).And.Contains(Follower1);
        }
    }
}
