using System.Collections.Generic;
using System.Linq;
using Mixter.Domain.Core.Messages;
using Mixter.Domain.Identity;

namespace Mixter.Infrastructure
{
    public class TimelineMessageRepository : ITimelineMessageRepository
    {
        private readonly List<TimelineMessageProjection> _messages = new List<TimelineMessageProjection>();

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
