using Mixter.Domain;

namespace Mixter.Infrastructure
{
    public class EventPublisherWithStorage : IEventPublisher
    {
        private readonly EventsStore _store;
        private readonly IEventPublisher _publisher;

        public EventPublisherWithStorage(EventsStore store, IEventPublisher publisher)
        {
            _store = store;
            _publisher = publisher;
        }

        public void Publish<TEvent>(TEvent evt) where TEvent : IDomainEvent
        {
            _store.Store(evt);
            _publisher.Publish(evt);
        }
    }
}
