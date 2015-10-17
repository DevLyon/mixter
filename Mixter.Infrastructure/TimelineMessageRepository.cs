using System.Collections.Generic;
using System.Linq;
using Mixter.Domain.Core.Messages;
using Mixter.Domain.Identity;

namespace Mixter.Infrastructure
{
    public class TimelineMessageRepository : ITimelineMessageRepository
    {
        private readonly HashSet<TimelineMessageProjection> _messages = new HashSet<TimelineMessageProjection>();

        public void Save(TimelineMessageProjection messageProjection)
        {
            _messages.Add(messageProjection);
        }

        public IEnumerable<TimelineMessageProjection> GetMessagesOfUser(UserId userId)
        {
            return _messages.Where(o => o.OwnerId.Equals(userId));
        }
    }
}
