using System.Collections.Generic;
using System.Linq;
using Mixter.Domain.Core.Messages;
using Mixter.Domain.Identity;

namespace Mixter.Infrastructure.Repositories
{
    public class TimelineMessagesRepository : ITimelineMessagesRepository
    {
        private readonly HashSet<TimelineMessage> _messages = new HashSet<TimelineMessage>();

        public void Save(TimelineMessage message)
        {
            _messages.Add(message);
        }

        public IEnumerable<TimelineMessage> GetMessagesOfUser(UserId userId)
        {
            return _messages.Where(o => o.OwnerId.Equals(userId));
        }
    }
}
