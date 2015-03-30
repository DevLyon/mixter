namespace Mixter.Domain.Core.Messages
{
    public interface IMessagesRepository
    {
        Message Get(MessageId id);
    }
}
