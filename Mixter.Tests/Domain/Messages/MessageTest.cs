using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mixter.Domain;
using Mixter.Domain.Messages;
using Mixter.Tests.Infrastructure;
using NFluent;

namespace Mixter.Tests.Domain.Messages
{
    [TestClass]
    public class MessageTest
    {
        private const string MessageContent = "Hello";
        private const string ReplyContent = "ReplyContent";

        private static readonly UserId Author = new UserId("pierre@mixit.fr");
        private static readonly UserId Republisher = new UserId("alfred@mixit.fr");
        private static readonly MessageId MessageId = MessageId.Generate();
        private static readonly UserId Replier = new UserId("jean@mixit.fr");

        private EventPublisherFake _eventPublisher;

        [TestInitialize]
        public void Initialize()
        {
            _eventPublisher = new EventPublisherFake();
        }

        [TestMethod]
        public void WhenPublishMessageThenRaiseUserMessagePublished()
        {
            Message.PublishMessage(_eventPublisher, Author, MessageContent);

            var evt = (MessagePublished)_eventPublisher.Events.First();
            Check.That(evt.Content).IsEqualTo(MessageContent);
        }

        [TestMethod]
        public void WhenRepublishMessageThenRaiseMessageRepublished()
        {
            var message = CreateMessage(
                new MessagePublished(MessageId, Author, MessageContent));

            message.RepublishMessage(_eventPublisher, Republisher);

            Check.That(_eventPublisher.Events).Contains(new MessageRepublished(message.GetId(), Republisher));
        }

        [TestMethod]
        public void WhenRepublishMyOwnMessageThenDoNotRaiseMessageRepublished()
        {
            var message = CreateMessage(
                new MessagePublished(MessageId, Author, MessageContent));

            message.RepublishMessage(_eventPublisher, Author);

            Check.That(_eventPublisher.Events).IsEmpty();
        }

        [TestMethod]
        public void WhenRepublishTwoTimesSameMessageThenDoNotRaiseMessageRepublished()
        {
            var message = CreateMessage(
                new MessagePublished(MessageId, Author, MessageContent), 
                new MessageRepublished(MessageId, Republisher));

            message.RepublishMessage(_eventPublisher, Republisher);

            Check.That(_eventPublisher.Events).IsEmpty();
        }

        [TestMethod]
        public void WhenReplyThenRaiseReplyMessagePublished()
        {
            var message = CreateMessage(
                new MessagePublished(MessageId, Author, MessageContent));

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
                new MessagePublished(MessageId, Author, MessageContent));

            message.Delete(_eventPublisher, Author);

            Check.That(_eventPublisher.Events).ContainsExactly(new MessageDeleted(MessageId));
        }

        [TestMethod]
        public void WhenDeleteBySomeoneElseThanAuthorThenDoNotRaiseMessageDeleted()
        {
            var message = CreateMessage(
                new MessagePublished(MessageId, Author, MessageContent));

            message.Delete(_eventPublisher, new UserId("clement@mix-it.fr"));

            Check.That(_eventPublisher.Events).IsEmpty();
        }

        [TestMethod]
        public void GivenIsRepublishedWhenDeleteByRepublisherThenDoNotRaiseMessageDeleted()
        {
            var message = CreateMessage(
                new MessagePublished(MessageId, Author, MessageContent),
                new MessageRepublished(MessageId, Republisher));

            message.Delete(_eventPublisher, Republisher);

            Check.That(_eventPublisher.Events).IsEmpty();
        }

        [TestMethod]
        public void GivenReplyMessageWhenGetIdHasReplayMessageId()
        {
            var replyMessageId = MessageId.Generate();
            var message = CreateMessage(
                new ReplyMessagePublished(replyMessageId, Replier, ReplyContent, MessageId));

            Check.That(message.GetId()).IsEqualTo(replyMessageId);
        }

        [TestMethod]
        public void GivenADeletedMessageWhenReplyThenDoNotRaiseMessageDeleted()
        {
            var message = CreateMessage(
                new MessagePublished(MessageId, Author, MessageContent),
                new MessageDeleted(MessageId));

            message.Reply(_eventPublisher, Replier, ReplyContent);

            Check.That(_eventPublisher.Events).IsEmpty();
        }

        private Message CreateMessage(params IDomainEvent[] events)
        {
            return new Message(events);
        }
    }
}