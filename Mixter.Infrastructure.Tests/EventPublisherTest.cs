using Mixter.Domain;
using NFluent;
using Xunit;

namespace Mixter.Infrastructure.Tests
{
    public class EventPublisherTest
    {
        [Fact]
        public void GivenHandlerWhenPublishThenCallHandler()
        {
            var handler = new EventHandler<EventA>();
            var publisher = new EventPublisher();
            publisher.Subscribe(handler);

            publisher.Publish(new EventA());

            Check.That(handler.IsCalled).IsTrue();
        }

        [Fact]
        public void GivenDifferentHandlersWhenPublishThenCallRightHandler()
        {
            var handlerB = new EventHandler<EventB>();
            var handlerA = new EventHandler<EventA>();
            var publisher = new EventPublisher();
            publisher.Subscribe(handlerA);
            publisher.Subscribe(handlerB);

            publisher.Publish(new EventB());

            Check.That(handlerA.IsCalled).IsFalse();
            Check.That(handlerB.IsCalled).IsTrue();
        }

        private class EventHandler<TEvent> : IEventHandler<TEvent> where TEvent : IDomainEvent
        {
            public bool IsCalled { get; private set; }

            public void Handle(TEvent evt)
            {
                IsCalled = true;
            }
        }

        private class EventA : IDomainEvent
        {
            public object GetAggregateId()
            {
                return "A";
            }
        }

        private class EventB : IDomainEvent
        {
            public object GetAggregateId()
            {
                return "B";
            }
        }
    }
}
