using System.Collections.Generic;
using System.Linq;
using Mixter.Domain;
using Mixter.Domain.Core.Messages;
using Mixter.Domain.Core.Messages.Events;

namespace Mixter.Infrastructure
{
    public class MessagesRepository
    {
        private readonly EventsStore _eventsStore;

        public MessagesRepository(EventsStore eventsStore)
        {
            _eventsStore = eventsStore;
        }

        public Message Get(MessageId id)
        {
            var events = _eventsStore.GetEventsOfAggregate(id).ToArray();
            if (!events.Any())
            {
                throw new UnknownMessage(id);
            }

            return new Message(events);
        }

        public MessageDescription GetDescription(MessageId id)
        {
            var creationEvent = _eventsStore.GetEventsOfAggregate(id).OfType<MessageQuacked?>().FirstOrDefault();
            if (!creationEvent.HasValue)
            {
                throw new UnknownMessage(id);
            }

            return new MessageDescription(creationEvent.Value);
        }
    }
}
