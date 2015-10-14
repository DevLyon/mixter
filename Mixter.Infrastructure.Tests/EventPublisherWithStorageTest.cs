using Mixter.Domain;
using Mixter.Domain.Tests;
using NFluent;
using Xunit;

namespace Mixter.Infrastructure.Tests
{
    public class EventPublisherWithStorageTest
    {
        private readonly EventsStore _store;
        private readonly EventPublisherWithStorage _publisher;
        private readonly EventPublisherFake _publisherBase;

        public EventPublisherWithStorageTest()
        {
            _store = new EventsStore();
            _publisherBase = new EventPublisherFake();
            _publisher = new EventPublisherWithStorage(_store, _publisherBase);
        }

        [Fact]
        public void WhenPublishEventThenStoreInDatabase()
        {
            _publisher.Publish(new EventA());

            Check.That(_store.GetEventsOfAggregate("A")).HasSize(1);
        }

        [Fact]
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
