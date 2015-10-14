using System.Collections.Generic;
using System.Linq;
using Mixter.Domain;

namespace Mixter.Infrastructure
{
    public class EventsStore
    {
        private readonly IList<IDomainEvent> _events = new List<IDomainEvent>();

        public void Store(IDomainEvent evt)
        {
            _events.Add(evt);
        }

        public IEnumerable<IDomainEvent> GetEventsOfAggregate<TAggregateId>(TAggregateId id)
        {
            return _events.Where(o => o.GetAggregateId().Equals(id));
        }
    }
}
