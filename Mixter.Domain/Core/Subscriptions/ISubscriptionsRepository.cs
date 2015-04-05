namespace Mixter.Domain.Core.Subscriptions
{
    public interface ISubscriptionsRepository
    {
        Subscription Get(SubscriptionId subscriptionId);
    }
}