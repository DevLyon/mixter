using System.Collections.Generic;
using System.Linq;
using Mixter.Domain;

namespace Mixter.Infrastructure
{
    public class EventsDatabase
    {
        private readonly IList<IDomainEvent> _events = new List<IDomainEvent>();

        public IEnumerable<IDomainEvent> GetEventsOfAggregate<TAggregate>(TAggregate id)
        {
            return _events.Where(o => o.GetAggregateId().Equals(id));
        }

        public void Store(IDomainEvent evt)
        {
            _events.Add(evt);
        }
    }
}