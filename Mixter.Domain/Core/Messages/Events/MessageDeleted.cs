using Mixter.Domain.Identity;

namespace Mixter.Domain.Core.Messages.Events
{
    [Event]
    public struct MessageDeleted : IDomainEvent
    {
        public MessageId MessageId { get; private set; }

        public UserId Deleter { get; private set; }

        public MessageDeleted(MessageId messageId, UserId deleter) 
            : this()
        {
            MessageId = messageId;
            Deleter = deleter;
        }

        public object GetAggregateId()
        {
            return MessageId;
        }
    }
}
