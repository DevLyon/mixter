using System.Collections.Generic;
using Mixter.Domain.Identity;

namespace Mixter.Domain.Core.Messages
{
    [Repository]
    public interface ITimelineMessageRepository
    {
        void Save(TimelineMessageProjection messageProjection);

        [Query]
        IEnumerable<TimelineMessageProjection> GetMessagesOfUser(UserId userId);
    }
}