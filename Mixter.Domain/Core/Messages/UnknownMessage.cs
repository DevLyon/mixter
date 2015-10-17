namespace Mixter.Domain.Core.Messages
{
    public class UnknownMessage : DomainException
    {
        public MessageId MessageId { get; private set; }

        public UnknownMessage(MessageId messageId)
            : base("Unknown message with id " + messageId)
        {
            MessageId = messageId;
        }
    }
}