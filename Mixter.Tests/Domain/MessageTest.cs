using System.Collections;
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

        private EventPublisherFake _eventPublisher;

        [TestInitialize]
        public void Initialize()
        {
            _eventPublisher = new EventPublisherFake();
        }

        [TestMethod]
        public void WhenPublishMessageThenRaiseUserMessagePublished()
        {
            Message.PublishMessage(_eventPublisher, MessageContent);

            var evt = (MessagePublished)_eventPublisher.Events.First();
            Check.That(evt.Message).IsEqualTo(MessageContent);
        }

        [TestMethod]
        public void WhenRepublishMessageThenRaiseMessageRepublished()
        {
            var message = Message.PublishMessage(_eventPublisher, MessageContent);

            message.RepublishMessage(_eventPublisher);

            Check.That(_eventPublisher.Events).Contains(new MessageRepublished(message.GetId()));
        }

        [TestMethod]
        public void WhenRepublishMyOwnMessageThenDoNotRaiseMessageRepublished()
        {
            Assert.Inconclusive();
            //var message = Message.PublishMessage(_eventPublisher, MessageContent, UserId("pierre@mixit.fr"));

            //message.RepublishMessage(_eventPublisher, UserId("pierre@mixit.fr"));

            //Check.That(_eventPublisher.Events).Not.Contains(new MessageRepublished(message.GetId()));
        }
    }
}