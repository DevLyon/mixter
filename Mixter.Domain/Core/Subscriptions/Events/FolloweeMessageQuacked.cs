using Mixter.Domain.Core.Messages;

namespace Mixter.Domain.Core.Subscriptions.Events
{
    [Event]
    public struct FolloweeMessageQuacked : IDomainEvent
    {
        public SubscriptionId SubscriptionId { get; private set; }

        public MessageId MessageId { get; private set; }

        public FolloweeMessageQuacked(SubscriptionId subscriptionId, MessageId messageId)
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