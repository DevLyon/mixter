namespace Mixter.Domain.Core.Subscriptions.Events
{
    public struct UserUnfollowed : IDomainEvent
    {
        public SubscriptionId SubscriptionId { get; private set; }

        public UserUnfollowed(SubscriptionId subscriptionId) : this()
        {
            SubscriptionId = subscriptionId;
        }

        public object GetAggregateId()
        {
            return SubscriptionId;
        }
    }
}