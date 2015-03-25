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
        private const string ReplyContent = "ReplyContent";

        private EventPublisherFake _eventPublisher;

        private readonly UserId _author = new UserId("pierre@mixit.fr");

        private readonly UserId _republisher = new UserId("alfred@mixit.fr");
        private static readonly MessageId MessageId = MessageId.Generate();
        private static readonly UserId Replier = new UserId("jean@mixit.fr");

        [TestInitialize]
        public void Initialize()
        {
            _eventPublisher = new EventPublisherFake();
        }

        [TestMethod]
        public void WhenPublishMessageThenRaiseUserMessagePublished()
        {
            Message.PublishMessage(_eventPublisher, _author, MessageContent);

            var evt = (MessagePublished)_eventPublisher.Events.First();
            Check.That(evt.Content).IsEqualTo(MessageContent);
        }

        private Message CreateMessage(params IDomainEvent[] events)
        {
            return new Message(events);
        }

        [TestMethod]
        public void WhenRepublishMessageThenRaiseMessageRepublished()
        {
            var message = CreateMessage(
                new MessagePublished(MessageId, _author, MessageContent));

            message.RepublishMessage(_eventPublisher, _republisher);

            Check.That(_eventPublisher.Events).Contains(new MessageRepublished(message.GetId(), _republisher));
        }

        [TestMethod]
        public void WhenRepublishMyOwnMessageThenDoNotRaiseMessageRepublished()
        {
            var message = CreateMessage(
                new MessagePublished(MessageId, _author, MessageContent));

            message.RepublishMessage(_eventPublisher, _author);

            Check.That(_eventPublisher.Events).IsEmpty();
        }

        [TestMethod]
        public void WhenRepublishTwoTimesSameMessageThenDoNotRaiseMessageRepublished()
        {
            var message = CreateMessage(
                new MessagePublished(MessageId, _author, MessageContent), 
                new MessageRepublished(MessageId, _republisher));

            message.RepublishMessage(_eventPublisher, _republisher);

            Check.That(_eventPublisher.Events).IsEmpty();
        }

        [TestMethod]
        public void WhenReplyThenRaiseReplyMessagePublished()
        {
            var message = CreateMessage(
                new MessagePublished(MessageId, _author, MessageContent));

            message.Reply(_eventPublisher, Replier, ReplyContent);

            var evt = _eventPublisher.Events.OfType<ReplyMessagePublished>().First();
            Check.That(evt.ParentId).IsEqualTo(MessageId);
            Check.That(evt.ReplyContent).IsEqualTo(ReplyContent);
            Check.That(evt.Replier).IsEqualTo(Replier);
            Check.That(evt.ReplyId).IsNotEqualTo(MessageId);
        }

        [TestMethod]
        public void WhenDeleteThenRaiseMessageDeleted()
        {
            var message = CreateMessage(
                new MessagePublished(MessageId, _author, MessageContent));

            message.Delete(_eventPublisher, _author);

            Check.That(_eventPublisher.Events).ContainsExactly(new MessageDeleted(MessageId));
        }

        [TestMethod]
        public void WhenDeleteBySomeoneElseThanAuthorThenDoNotRaiseMessageDeleted()
        {
            var message = CreateMessage(
                new MessagePublished(MessageId, _author, MessageContent));

            message.Delete(_eventPublisher, new UserId("clement@mix-it.fr"));

            Check.That(_eventPublisher.Events).IsEmpty();
        }

        [TestMethod]
        public void GivenIsRepublishedWhenDeleteByRepublisherThenDoNotRaiseMessageDeleted()
        {
            var message = CreateMessage(
                new MessagePublished(MessageId, _author, MessageContent),
                new MessageRepublished(MessageId, _republisher));

            message.Delete(_eventPublisher, _republisher);

            Check.That(_eventPublisher.Events).IsEmpty();
        }
    }
}