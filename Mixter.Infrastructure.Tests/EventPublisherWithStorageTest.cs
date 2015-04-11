using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mixter.Domain;
using NFluent;

namespace Mixter.Infrastructure.Tests
{
    [TestClass]
    public class EventPublisherWithStorageTest
    {
        private EventsStore _store;
        private EventPublisherWithStorage _publisher;
        private EventPublisherFake _publisherBase;

        [TestInitialize]
        public void Initialize()
        {
            _store = new EventsStore();
            _publisherBase = new EventPublisherFake();
            _publisher = new EventPublisherWithStorage(_store, _publisherBase);
        }

        [TestMethod]
        public void WhenPublishEventThenStoreInDatabase()
        {
            _publisher.Publish(new EventA());

            Check.That(_store.GetEventsOfAggregate("A")).HasSize(1);
        }

        [TestMethod]
        public void WhenPublishEventThenCallEventHandlerBase()
        {
            _publisher.Publish(new EventA());

            Check.That(_publisherBase.Events).HasSize(1);
        }

        private struct EventA : IDomainEvent
        {
            public object GetAggregateId()
            {
                return "A";
            }
        }
    }
}