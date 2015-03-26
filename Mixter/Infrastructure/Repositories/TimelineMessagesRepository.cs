using System.Collections.Generic;
using System.Linq;
using Mixter.Domain;
using Mixter.Domain.Messages;

namespace Mixter.Infrastructure.Repositories
{
    public class TimelineMessagesRepository : ITimelineMessagesRepository
    {
        private readonly IList<TimelineMessage> _messages = new List<TimelineMessage>();

        public void Save(TimelineMessage message)
        {
            if (_messages.Contains(message))
            {
                return;
            }

            _messages.Add(message);
        }

        public IEnumerable<TimelineMessage> GetMessagesOfUser(UserId userId)
        {
            return _messages.Where(o => o.OwnerId.Equals(userId));
        }
    }
}
