namespace Mixter.Domain.Subscriptions
{
    public class Subscription
    {
        public static void FollowUser(IEventPublisher eventPublisher, UserId follower, UserId followee)
        {
            eventPublisher.Publish(new UserFollowed(new SubscriptionId(follower, followee)));
        }
    }
}