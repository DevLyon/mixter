using Mixter.Domain.Messages;

namespace Mixter.Domain.Subscriptions
{
    public class Subscription
    {
        private readonly DecisionProjection _projection;

        public Subscription(UserFollowed evt)
        {
            _projection = new DecisionProjection();
            _projection.Apply(evt);
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
            eventPublisher.Publish(new FollowerMessagePublished(_projection.Id, messageId));
        }

        private class DecisionProjection
        {
            public SubscriptionId Id { get; private set; }

            public void Apply(UserFollowed evt)
            {
                Id = evt.SubscriptionId;
            }
        }
    }
}