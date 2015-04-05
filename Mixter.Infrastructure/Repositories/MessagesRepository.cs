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
            var evt = _eventsDatabase.GetEventsOfAggregate(id).OfType<MessagePublished>().First();
            return new MessageDescription(evt);
        }
    }
}