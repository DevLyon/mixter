using Mixter.Domain.Core.Messages;

namespace Mixter.Domain.Core.Subscriptions.Events
{
    public struct FolloweeMessagePublished : IDomainEvent
    {
        public SubscriptionId SubscriptionId { get; private set; }

        public MessageId MessageId { get; private set; }

        public string Content { get; private set; }

        public FolloweeMessagePublished(SubscriptionId subscriptionId, MessageId messageId, string content)
            : this()
        {
            SubscriptionId = subscriptionId;
            MessageId = messageId;
            Content = content;
        }

        public object GetAggregateId()
        {
            return SubscriptionId;
        }
    }
}