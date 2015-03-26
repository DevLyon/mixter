namespace Mixter.Domain.Subscriptions
{
    public struct UserFollowed : IDomainEvent
    {
        public SubscriptionId SubscriptionId { get; private set; }

        public UserFollowed(SubscriptionId subscriptionId)
            : this()
        {
            SubscriptionId = subscriptionId;
        }

        public object GetAggregateId()
        {
            return SubscriptionId;
        }
    }
}