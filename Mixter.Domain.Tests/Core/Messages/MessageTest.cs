using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mixter.Domain.Core.Messages;
using Mixter.Domain.Core.Messages.Events;
using Mixter.Domain.Identity;
using Mixter.Infrastructure;
using Mixter.Infrastructure.Tests.Infrastructure;
using NFluent;

namespace Mixter.Domain.Tests.Core.Messages
{
    [TestClass]
    public class MessageTest
    {
        private const string MessageContent = "Hello";
        private const string ReplyContent = "ReplyContent";

        private static readonly UserId Author = new UserId("pierre@mixit.fr");
        private static readonly UserId Republisher = new UserId("alfred@mixit.fr");
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
            Message.Publish(_eventPublisher, Author, MessageContent);

            var evt = (MessagePublished)_eventPublisher.Events.First();
            Check.That(evt.Content).IsEqualTo(MessageContent);
        }

        [TestMethod]
        public void WhenRepublishMessageThenRaiseMessageRepublished()
        {
            var message = Message.Publish(new EventPublisher(), Author, MessageContent);
            message.Republish(_eventPublisher, Republisher);
            Check.That(_eventPublisher.Events).ContainsExactly(new MessageRepublished(message.GetId(), Republisher));
        }

        [TestMethod]
        public void WhenRepublishMyOwnMessageThenDoNotRaiseMessageRepublished()
        {
            var message = Message.Publish(new EventPublisher(), Author, MessageContent);
            message.Republish(_eventPublisher, Author);
            Check.That(_eventPublisher.Events).IsEmpty();
        }

        [TestMethod]
        public void WhenRepublishTwoTimesSameMessageThenDoNotRaiseMessageRepublished()
        {
            var message = Message.Publish(new EventPublisher(), Author, MessageContent);
            message.Republish(_eventPublisher, Republisher);
            message.Republish(_eventPublisher, Republisher);
            Check.That(_eventPublisher.Events).ContainsExactly(new MessageRepublished(message.GetId(), Republisher));
        }

        [TestMethod]
        public void WhenReplyThenRaiseReplyMessagePublished()
        {
            var message = Message.Publish(_eventPublisher, Author, MessageContent);
            message.Reply(_eventPublisher, Replier, ReplyContent);
            var replyEvent = _eventPublisher.Events.OfType<ReplyMessagePublished>().First();
            Check.That(replyEvent.ParentId).IsEqualTo(message.GetId());
            Check.That(replyEvent.ReplyContent).IsEqualTo(ReplyContent);
            Check.That(replyEvent.Replier).IsEqualTo(Replier);
            Check.That(replyEvent.ReplyId).IsNotEqualTo(message.GetId());
        }
        
        [TestMethod]
        public void WhenDeleteThenRaiseMessageDeleted()
        {
            var message = Message.Publish(new EventPublisher(), Author, MessageContent);
            message.Delete(_eventPublisher, Author);
            Check.That(_eventPublisher.Events).ContainsExactly(new MessageDeleted(message.GetId()));
        }

        [TestMethod]
        public void WhenDeleteBySomeoneElseThanAuthorThenDoNotRaiseMessageDeleted()
        {
            var semeoneElseThanAuthor = new UserId("someone@mixit.fr");
            var message = Message.Publish(new EventPublisher(), Author, MessageContent);
            message.Delete(_eventPublisher, semeoneElseThanAuthor);
            Check.That(_eventPublisher.Events).IsEmpty();
        }

        [TestMethod]
        public void GivenDeletedMessageWhenDeleteThenNothing()
        {
            var message = Message.Publish(new EventPublisher(), Author, MessageContent);
            message.Delete(new EventPublisher(), Author);
            message.Delete(_eventPublisher, Author);
            Check.That(_eventPublisher.Events).IsEmpty();
        }

        [TestMethod]
        public void GivenADeletedMessageWhenReplyThenNothing()
        {
            var message = Message.Publish(new EventPublisher(), Author, MessageContent);
            message.Delete(new EventPublisher(), Author);
            message.Reply(_eventPublisher, Replier, ReplyContent);
            Check.That(_eventPublisher.Events).IsEmpty();
        }

        [TestMethod]
        public void GivenDeletedMessageWhenRepublishThenNothing()
        {
            var message = Message.Publish(new EventPublisher(), Author, MessageContent);
            message.Delete(new EventPublisher(), Author);
            message.Republish(_eventPublisher, Republisher);
            Check.That(_eventPublisher.Events).IsEmpty();
        }
    }
}