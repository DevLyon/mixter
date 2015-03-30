using Mixter.Domain.Identity;

namespace Mixter.Domain.Core.Messages
{
    public struct TimelineMessageId
    {
        public UserId Owner { get; private set; }

        public MessageId MessageId { get; private set; }

        public TimelineMessageId(UserId owner, MessageId messageId)
            : this()
        {
            Owner = owner;
            MessageId = messageId;
        }
    }
}