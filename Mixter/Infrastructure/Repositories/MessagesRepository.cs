using Mixter.Domain.Messages;

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
    }
}