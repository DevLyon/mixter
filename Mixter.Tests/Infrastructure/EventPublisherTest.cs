using System;
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
            var handler = new EventAHandler();
            var publisher = new EventPublisher(handler);

            publisher.Publish(new EventA());

            Check.That(handler.IsCalled).IsTrue();
        }
    }
}
