using Mixter.Domain;

namespace Mixter.Infrastructure
{
    public class EventPublisherWithStorage : IEventPublisher
    {
        private readonly EventsDatabase _database;
        private readonly IEventPublisher _publisher;

        public EventPublisherWithStorage(EventsDatabase database, IEventPublisher publisher)
        {
            _database = database;
            _publisher = publisher;
        }

        public void Publish<TEvent>(TEvent evt) where TEvent : IDomainEvent
        {
            _database.Store(evt);
            _publisher.Publish(evt);
        }
    }
}
