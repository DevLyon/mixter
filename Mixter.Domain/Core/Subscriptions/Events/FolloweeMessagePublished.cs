using Mixter.Domain.Core.Messages;

namespace Mixter.Domain.Core.Subscriptions.Events
{
    public struct FolloweeMessagePublished : IDomainEvent
    {
        public SubscriptionId SubscriptionId { get; private set; }

        public MessageId MessageId { get; private set; }

        public FolloweeMessagePublished(SubscriptionId subscriptionId, MessageId messageId)
            : this()
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