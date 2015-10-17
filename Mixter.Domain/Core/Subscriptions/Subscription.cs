using System.Collections.Generic;
using Mixter.Domain.Core.Messages;
using Mixter.Domain.Core.Subscriptions.Events;
using Mixter.Domain.Identity;

namespace Mixter.Domain.Core.Subscriptions
{
    [Aggregate]
    public class Subscription
    {
        private readonly DecisionProjection _projection;

        public Subscription(IEnumerable<IDomainEvent> events)
        {
            _projection = new DecisionProjection();

            foreach (var evt in events)
            {
                _projection.Apply(evt);
            }
        }

        [Command]
        public static void FollowUser(IEventPublisher eventPublisher, UserId follower, UserId followee)
        {
            var userFollowed = new UserFollowed(new SubscriptionId(follower, followee));
            eventPublisher.Publish(userFollowed);
        }

        [Command]
        public void Unfollow(IEventPublisher eventPublisher)
        {
            eventPublisher.Publish(new UserUnfollowed(_projection.Id));
        }

        [Command]
        public void NotifyFollower(IEventPublisher eventPublisher, MessageId messageId)
        {
            eventPublisher.Publish(new FolloweeMessageQuacked(_projection.Id, messageId));
        }

        [Projection]
        private class DecisionProjection : DecisionProjectionBase
        {
            public DecisionProjection()
            {
                AddHandler<UserFollowed>(When);
            }

            public SubscriptionId Id { get; private set; }

            private void When(UserFollowed evt)
            {
                Id = evt.SubscriptionId;
            }
        }
    }
}
