using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mixter.Domain.Core.Messages;
using Mixter.Domain.Core.Subscriptions;
using Mixter.Domain.Core.Subscriptions.Events;
using Mixter.Domain.Identity;
using Mixter.Infrastructure.Tests;
using NFluent;

namespace Mixter.Domain.Tests.Core.Subscriptions
{
    [TestClass]
    public class SubscriptionTest
    {
        private static readonly UserId Follower = new UserId("emilien@mixit.fr");
        private static readonly UserId Followee = new UserId("florent@mixit.fr");
        private static readonly SubscriptionId SubscriptionId = new SubscriptionId(Follower, Followee);

        private EventPublisherFake _eventPublisher;

        [TestInitialize]
        public void Initialize()
        {
            _eventPublisher = new EventPublisherFake();
        }

        [TestMethod]
        public void WhenFollowThenUserFollowedIsRaised()
        {
            Subscription.FollowUser(_eventPublisher, Follower, Followee);

            Check.That(_eventPublisher.Events).Contains(new UserFollowed(SubscriptionId));
        }

        [TestMethod]
        public void WhenUnfollowThenUserUnfollowedIsRaised()
        {
            var subscription = Create(new UserFollowed(SubscriptionId));

            subscription.Unfollow(_eventPublisher);

            Check.That(_eventPublisher.Events).Contains(new UserUnfollowed(SubscriptionId));
        }

        [TestMethod]
        public void WhenNotifyFollowerThenFolloweeMessagePublished()
        {
            var subscription = Create(new UserFollowed(SubscriptionId));

            var messageId = MessageId.Generate();
            subscription.NotifyFollower(_eventPublisher, messageId);

            Check.That(_eventPublisher.Events).Contains(new FolloweeMessagePublished(SubscriptionId, messageId));
        }

        private Subscription Create(params IDomainEvent[] events)
        {
            return new Subscription(events);
        }
    }
}
