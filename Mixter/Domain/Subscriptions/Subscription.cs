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
            eventPublisher.Publish(new UserFollowed(new SubscriptionId(follower, followee)));
        }

        public void Unfollow(IEventPublisher eventPublisher)
        {
            eventPublisher.Publish(new UserUnfollowed(_projection.Id));
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