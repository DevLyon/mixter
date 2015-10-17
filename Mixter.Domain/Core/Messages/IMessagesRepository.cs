namespace Mixter.Domain.Core.Messages
{
    [Repository]
    public interface IMessagesRepository
    {
        [Query]
        Message Get(MessageId id);

        [Query]
        MessageDescription GetDescription(MessageId id);
    }
}