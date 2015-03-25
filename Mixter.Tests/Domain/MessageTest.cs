using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mixter.Domain;
using NFluent;

namespace Mixter.Tests.Domain
{
    [TestClass]
    public class MessageTest
    {
        [TestMethod]
        public void WhenPublishMessageThenRaiseUserMessagePublished()
        {
            var eventPublisher = new EventPublisherFake();

            Message.PublishMessage(eventPublisher, "Hello");

            var evt = (MessagePublished)eventPublisher.Events.First();
            Check.That(evt.Message).IsEqualTo("Hello");
        }
    }
}