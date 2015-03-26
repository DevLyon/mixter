using Mixter.Domain.Messages;

namespace Mixter.Domain.Subscriptions.Events
{
    public struct FollowerMessagePublished : IDomainEvent
    {
        public SubscriptionId SubscriptionId { get; private set; }
        
        public MessageId MessageId { get; private set; }

        public FollowerMessagePublished(SubscriptionId subscriptionId, MessageId messageId) : this()
        {
            SubscriptionId = subscriptionId;
            MessageId = messageId;
        }

        public object GetAggregateId()
        {
            return SubscriptionId;
        }
    }
}