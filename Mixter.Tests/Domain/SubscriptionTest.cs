using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mixter.Domain;
using NFluent;

namespace Mixter.Tests.Domain
{
    [TestClass]
    public class SubscriptionTest
    {
        private EventPublisherFake _eventPublisher;

        [TestInitialize]
        public void Initialize()
        {
            _eventPublisher = new EventPublisherFake();
        }

        [TestMethod]
        public void WhenFollowThenUserFollowedIsRaised()
        {
            var follower = new UserId("emilien@mixit.fr");
            var followee = new UserId("florent@mixit.fr");
            Subscription.FollowUser(_eventPublisher, follower, followee);

            Check.That(_eventPublisher.Events).Contains(new UserFollowed(new SubscriptionId(follower, followee)));
        }
    }
}
