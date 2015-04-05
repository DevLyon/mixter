namespace Mixter.Domain.Core.Messages
{
    public interface IMessagesRepository
    {
        Message Get(MessageId id);

        MessageDescription GetDescription(MessageId id);
    }
}
