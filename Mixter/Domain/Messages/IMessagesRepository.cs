namespace Mixter.Domain.Messages
{
    public interface IMessagesRepository
    {
        Message Get(MessageId id);
    }
}
