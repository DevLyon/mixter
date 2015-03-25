using System.Collections.Generic;
using Mixter.Domain.Messages;

namespace Mixter.Domain.Subscriptions
{
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

        public static void FollowUser(IEventPublisher eventPublisher, UserId follower, UserId followee)
        {
            var userFollowed = new UserFollowed(new SubscriptionId(follower, followee));
            eventPublisher.Publish(userFollowed);
        }

        public void Unfollow(IEventPublisher eventPublisher)
        {
            eventPublisher.Publish(new UserUnfollowed(_projection.Id));
        }

        public void NotifyFollower(IEventPublisher eventPublisher, MessageId messageId)
        {
            if (_projection.IsUnfollow)
            {
                return;
            }

            eventPublisher.Publish(new FollowerMessagePublished(_projection.Id, messageId));
        }

        private class DecisionProjection
        {
            public SubscriptionId Id { get; private set; }

            public bool IsUnfollow { get; private set; }

            public void Apply(UserFollowed evt)
            {
                Id = evt.SubscriptionId;
            }

            public void Apply(UserUnfollowed evt)
            {
                IsUnfollow = true;
            }

            public void Apply(IDomainEvent evt)
            {
                if (evt is UserFollowed)
                {
                    Apply((UserFollowed) evt);
                }

                if (evt is UserUnfollowed)
                {
                    Apply((UserUnfollowed)evt);
                }
            }
        }
    }
}