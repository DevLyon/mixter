using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mixter.Domain;
using NFluent;

namespace Mixter.Infrastructure.Tests
{
    [TestClass]
    public class EventPublisherTest
    {
        [TestMethod]
        public void GivenHandlerWhenPublishThenCallHandler()
        {
            var handler = new EventHandler<EventA>();
            var publisher = new EventPublisher(handler);

            publisher.Publish(new EventA());

            Check.That(handler.IsCalled).IsTrue();
        }

        [TestMethod]
        public void GivenDifferentHandlersWhenPublishThenCallRightHandler()
        {
            var handlerB = new EventHandler<EventB>();
            var handlerA = new EventHandler<EventA>();
            var publisher = new EventPublisher(handlerA, handlerB);

            publisher.Publish(new EventB());

            Check.That(handlerA.IsCalled).IsFalse();
            Check.That(handlerB.IsCalled).IsTrue();
        }

        [TestMethod]
        public void WhenCreatePublisherWithMethodFactoryThenCreateHandlerWithPublisher()
        {
            var handlerB = new EventHandler<EventB>();
            var publisher = new EventPublisher(o => new IEventHandler[] { new EventHandlerWithPublisher(o), handlerB });

            publisher.Publish(new EventA());

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

        private class EventHandlerWithPublisher : IEventHandler<EventA>
        {
            private readonly IEventPublisher _eventPublisher;

            public EventHandlerWithPublisher(IEventPublisher eventPublisher)
            {
                _eventPublisher = eventPublisher;
            }

            public void Handle(EventA evt)
            {
                _eventPublisher.Publish(new EventB());
            }
        }
    }
}
