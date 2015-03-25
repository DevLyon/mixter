namespace Mixter.Domain
{
    public struct UserFollowed : IDomainEvent
    {
        public SubscriptionId SubscriptionId { get; private set; }

        public UserFollowed(SubscriptionId subscriptionId)
            : this()
        {
            SubscriptionId = subscriptionId;
        }
    }
}