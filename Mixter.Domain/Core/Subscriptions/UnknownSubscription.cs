namespace Mixter.Domain.Core.Subscriptions
{
    public class UnknownSubscription : DomainException
    {
        public SubscriptionId SubscriptionId { get; private set; }

        public UnknownSubscription(SubscriptionId subscriptionId)
            : base("Unknown subscription with id " + subscriptionId)
        {
            SubscriptionId = subscriptionId;
        }
    }
}
