using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mixter.Domain;
using Mixter.Infrastructure;
using NFluent;

namespace Mixter.Tests.Infrastructure
{
    [TestClass]
    public class EventPublisherWithStorageTest
    {
        private EventsDatabase _database;
        private EventPublisherWithStorage _publisher;
        private EventPublisherFake _publisherBase;

        [TestInitialize]
        public void Initialize()
        {
            _database = new EventsDatabase();
            _publisherBase = new EventPublisherFake();
            _publisher = new EventPublisherWithStorage(_database, _publisherBase);
        }

        [TestMethod]
        public void WhenPublishEventThenStoreInDatabase()
        {
            _publisher.Publish(new EventA());

            Check.That(_database.GetEventsOfAggregate("A")).HasSize(1);
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