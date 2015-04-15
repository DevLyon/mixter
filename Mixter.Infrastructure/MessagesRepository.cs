using System;
using System.Linq;
using Mixter.Domain.Core.Messages;
using Mixter.Domain.Core.Messages.Events;

namespace Mixter.Infrastructure
{
    public class MessagesRepository : IMessagesRepository
    {
        private readonly EventsStore _eventsStore;

        public MessagesRepository(EventsStore eventsStore)
        {
            _eventsStore = eventsStore;
        }

        public Message Get(MessageId id)
        {
            return new Message(_eventsStore.GetEventsOfAggregate(id));
        }

        public MessageDescription GetDescription(MessageId id)
        {
            var creationEvent = _eventsStore.GetEventsOfAggregate(id).First();

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