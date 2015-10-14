using System.Collections.Generic;

namespace Mixter.Domain.Tests
{
    public class EventPublisherFake : IEventPublisher
    {
        private readonly IList<IDomainEvent> _events = new List<IDomainEvent>();

        public IEnumerable<IDomainEvent> Events { get { return _events; } }

        public void Publish<TEvent>(TEvent evt) where TEvent : IDomainEvent
        {
            _events.Add(evt);
        }
    }
}