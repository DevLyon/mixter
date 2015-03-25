using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mixter.Domain;
using Mixter.Infrastructure;
using NFluent;

namespace Mixter.Tests.Infrastructure
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
        }

        private class EventB : IDomainEvent
        {
        }
    }
}
