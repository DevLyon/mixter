using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mixter.Domain;
using NFluent;

namespace Mixter.Tests.Domain
{
    [TestClass]
    public class MessageTest
    {
        private const string MessageContent = "Hello";

        [TestMethod]
        public void WhenPublishMessageThenRaiseUserMessagePublished()
        {
            var eventPublisher = new EventPublisherFake();

            Message.PublishMessage(eventPublisher, MessageContent);

            var evt = (MessagePublished)eventPublisher.Events.First();
            Check.That(evt.Message).IsEqualTo(MessageContent);
        }
    }
}