namespace Mixter.Domain.Core.Messages.Events
{
    public struct MessageDeleted : IDomainEvent
    {
        public MessageId MessageId { get; private set; }

        public MessageDeleted(MessageId messageId) : this()
        {
            MessageId = messageId;
        }

        public object GetAggregateId()
        {
            return MessageId;
        }
    }
}