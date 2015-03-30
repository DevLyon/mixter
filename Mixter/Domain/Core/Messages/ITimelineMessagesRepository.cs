using System.Collections.Generic;
using Mixter.Domain.Identity;

namespace Mixter.Domain.Core.Messages
{
    public interface ITimelineMessagesRepository
    {
        void Save(TimelineMessageProjection messageProjection);

        IEnumerable<TimelineMessageProjection> GetMessagesOfUser(UserId userId);
    }
}