namespace Mixter.Domain.Subscriptions
{
    public struct UserUnfollowed : IDomainEvent
    {
        public SubscriptionId SubscriptionId { get; private set; }

        public UserUnfollowed(SubscriptionId subscriptionId) : this()
        {
            SubscriptionId = subscriptionId;
        }
    }
}