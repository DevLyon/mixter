using System.Collections.Generic;
using Mixter.Domain.Identity;

namespace Mixter.Domain.Core.Messages
{
    public interface ITimelineMessageRepository
    {
        void Save(TimelineMessageProjection messageProjection);
        IEnumerable<TimelineMessageProjection> GetMessagesOfUser(UserId userId);
    }
}