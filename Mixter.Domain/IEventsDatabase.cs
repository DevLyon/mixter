using System.Collections.Generic;

namespace Mixter.Domain
{
    public interface IEventsDatabase
    {
        void Store(IDomainEvent evt);
        IEnumerable<IDomainEvent> GetEventsOfAggregate<TAggregateId>(TAggregateId id);
        IEnumerable<IDomainEvent> GetEvents();
    }
}