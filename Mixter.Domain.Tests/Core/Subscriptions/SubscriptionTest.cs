using Mixter.Domain.Core.Messages;
using Mixter.Domain.Core.Subscriptions;
using Mixter.Domain.Core.Subscriptions.Events;
using Mixter.Domain.Identity;
using NFluent;
using Xunit;

namespace Mixter.Domain.Tests.Core.Subscriptions
{
    public class SubscriptionTest
    {
        private static readonly UserId Follower = new UserId("emilien@mixit.fr");
        private static readonly UserId Followee = new UserId("florent@mixit.fr");
        private static readonly SubscriptionId SubscriptionId = new SubscriptionId(Follower, Followee);

        private readonly EventPublisherFake _eventPublisher;

        public SubscriptionTest()
        {
            _eventPublisher = new EventPublisherFake();
        }

        [Fact]
        public void WhenFollowThenUserFollowedIsRaised()
        {
            Subscription.FollowUser(_eventPublisher, Follower, Followee);

            Check.That(_eventPublisher.Events).Contains(new UserFollowed(SubscriptionId));
        }

        [Fact]
        public void WhenUnfollowThenUserUnfollowedIsRaised()
        {
            var subscription = Create(new UserFollowed(SubscriptionId));

            subscription.Unfollow(_eventPublisher);

            Check.That(_eventPublisher.Events).Contains(new UserUnfollowed(SubscriptionId));
        }

        [Fact]
        public void WhenNotifyFollowerThenFolloweeMessageQuacked()
        {
            var subscription = Create(new UserFollowed(SubscriptionId));

            var messageId = MessageId.Generate();
            subscription.NotifyFollower(_eventPublisher, messageId);

            Check.That(_eventPublisher.Events).Contains(new FolloweeMessageQuacked(SubscriptionId, messageId));
        }

        [Fact]
        public void GivenUnfollowWhenNotifyFollowerThenDoNotRaisedFollowerMessageQuacked()
        {
            var subscription = Create(
                new UserFollowed(SubscriptionId),
                new UserUnfollowed(SubscriptionId));

            var messageId = MessageId.Generate();
            subscription.NotifyFollower(_eventPublisher, messageId);

            Check.That(_eventPublisher.Events).IsEmpty();
        }

        private Subscription Create(params IDomainEvent[] events)
        {
            return new Subscription(events);
        }
    }
}
