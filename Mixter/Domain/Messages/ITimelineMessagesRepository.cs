namespace Mixter.Domain.Messages
{
    public interface ITimelineMessagesRepository
    {
        void Save(TimelineMessage message);
    }
}