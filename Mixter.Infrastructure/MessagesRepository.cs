using System.Linq;
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
            return new Message(_eventsStore.GetEventsOfAggregate(id));
        }

        public MessageDescription GetDescription(MessageId id)
        {
            var creationEvent = _eventsStore.GetEventsOfAggregate(id).OfType<MessageQuacked>().First();

            return new MessageDescription(creationEvent);
        }
    }
}
