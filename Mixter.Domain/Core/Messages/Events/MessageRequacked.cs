using Mixter.Domain.Identity;

namespace Mixter.Domain.Core.Messages.Events
{
    [Event]
    public struct MessageRequacked : IDomainEvent
    {
        public MessageId Id { get; private set; }

        public UserId Requacker { get; private set; }

        public MessageRequacked(MessageId id, UserId requacker)
            : this()
        {
            Id = id;
            Requacker = requacker;
        }

        public object GetAggregateId()
        {
            return Id;
        }
    }
}
