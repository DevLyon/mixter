using System.Collections.Generic;
using Mixter.Domain.Identity;

namespace Mixter.Domain.Core.Messages
{
    public interface ITimelineMessagesRepository
    {
        void Save(TimelineMessage message);

        IEnumerable<TimelineMessage> GetMessagesOfUser(UserId userId);
    }
}