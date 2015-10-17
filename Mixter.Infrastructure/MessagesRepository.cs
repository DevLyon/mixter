using Mixter.Domain.Core.Messages;

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
    }
}
