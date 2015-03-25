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

        private readonly UserId _creator = new UserId("pierre@mixit.fr");
        
        private readonly UserId _republisher = new UserId("jean@mixit.fr");

        [TestInitialize]
        public void Initialize()
        {
            _eventPublisher = new EventPublisherFake();
        }

        [TestMethod]
        public void WhenPublishMessageThenRaiseUserMessagePublished()
        {
            Message.PublishMessage(_eventPublisher, _creator, MessageContent);

            var evt = (MessagePublished)_eventPublisher.Events.First();
            Check.That(evt.Message).IsEqualTo(MessageContent);
        }

        [TestMethod]
        public void WhenRepublishMessageThenRaiseMessageRepublished()
        {
            var message = Message.PublishMessage(_eventPublisher, _creator, MessageContent);

            message.RepublishMessage(_eventPublisher, _republisher);

            Check.That(_eventPublisher.Events).Contains(new MessageRepublished(message.GetId(), _creator));
        }

        [TestMethod]
        public void WhenRepublishMyOwnMessageThenDoNotRaiseMessageRepublished()
        {
            var message = Message.PublishMessage(_eventPublisher, _creator, MessageContent);

            message.RepublishMessage(_eventPublisher, _creator);

            Check.That(_eventPublisher.Events).Not.Contains(new MessageRepublished(message.GetId(), _creator));
        }
    }
}