using System.Collections.Generic;

namespace Mixter.Domain.Messages
{
    public interface ITimelineMessagesRepository
    {
        void Save(TimelineMessage message);

        IEnumerable<TimelineMessage> GetMessagesOfUser(UserId userId);
    }
}