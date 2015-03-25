using System.Collections.Generic;
using Mixter.Domain;

namespace Mixter.Tests.Domain
{
    public class EventPublisherFake : IEventPublisher
    {
        private readonly IList<IDomainEvent> _events = new List<IDomainEvent>(); 

        public IEnumerable<IDomainEvent> Events { get { return _events; } }

        public void Publish(IDomainEvent evt)
        {
            _events.Add(evt);
        }
    }
}