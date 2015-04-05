using System;
using System.Linq;
using Mixter.Domain.Core.Messages;
using Mixter.Domain.Core.Messages.Events;

namespace Mixter.Infrastructure.Repositories
{
    public class MessagesRepository : IMessagesRepository
    {
        private readonly EventsDatabase _eventsDatabase;

        public MessagesRepository(EventsDatabase eventsDatabase)
        {
            _eventsDatabase = eventsDatabase;
        }

        public Message Get(MessageId id)
        {
            return new Message(_eventsDatabase.GetEventsOfAggregate(id));
        }

        public MessageDescription GetDescription(MessageId id)
        {
            var creationEvent = _eventsDatabase.GetEventsOfAggregate(id).First();

            if (creationEvent is MessagePublished)
            {
                return new MessageDescription((MessagePublished)creationEvent);
            }

            if (creationEvent is ReplyMessagePublished)
            {
                return new MessageDescription((ReplyMessagePublished)creationEvent);
            }
            
            throw new NotSupportedException("Unknown creation event of message " + id);
        }
    }
}