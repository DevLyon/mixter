using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mixter.Domain;
using NFluent;

namespace Mixter.Infrastructure.Tests
{
    [TestClass]
    public class EventsStoreTest
    {
        private static readonly AgregateAId AgregateId1 = new AgregateAId("A");
        private static readonly AgregateAId AgregateId2 = new AgregateAId("B");

        private EventsStore _store;

        [TestInitialize]
        public void Initialize()
        {
            _store = new EventsStore();
        }

        [TestMethod]
        public void WhenStoreEventOfAggregateThenCanGetThisEventOfAggregate()
        {
            _store.Store(new EventA(AgregateId1));

            Check.That(_store.GetEventsOfAggregate(AgregateId1)).HasSize(1);
        }

        [TestMethod]
        public void GivenEventsOfSeveralAggregatesWhenGetEventsOfAggregateThenReturnEventsOfOnlyThisAggregate()
        {
            _store.Store(new EventA(AgregateId1));
            _store.Store(new EventA(AgregateId2));
            _store.Store(new EventA(AgregateId1));

            var eventsOfAggregateA = _store.GetEventsOfAggregate(AgregateId1).ToArray();

            Check.That(eventsOfAggregateA).HasSize(2);
            Check.That(eventsOfAggregateA.Cast<EventA>().Select(o => o.Id).Distinct()).ContainsExactly(AgregateId1);
        }

        [TestMethod]
        public void GivenSeveralEventsWhenGetEventsOfAggregateThenReturnEventsAndPreserveOrder()
        {
            _store.Store(new EventA(AgregateId1, 1));
            _store.Store(new EventA(AgregateId1, 2));
            _store.Store(new EventA(AgregateId1, 3));

            var eventsOfAggregateA = _store.GetEventsOfAggregate(AgregateId1).ToArray();

            Check.That(eventsOfAggregateA.Cast<EventA>().Select(o => o.Value)).ContainsExactly(1, 2, 3);
        }

        private struct EventA : IDomainEvent
        {
            public AgregateAId Id { get; private set; }

            public int Value { get; private set; }

            public EventA(AgregateAId id, int value = 0)
                : this()
            {
                Id = id;
                Value = value;
            }

            public object GetAggregateId()
            {
                return Id;
            }
        }

        private struct AgregateAId
        {
            public string Id { get; private set; }

            public AgregateAId(string id)
                : this()
            {
                Id = id;
            }
        }
    }
}
