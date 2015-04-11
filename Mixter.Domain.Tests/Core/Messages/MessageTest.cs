using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mixter.Domain.Core.Messages;
using Mixter.Domain.Core.Messages.Events;
using Mixter.Domain.Identity;
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
            Message.Publish(_eventPublisher, Author, MessageContent);

            var evt = (MessagePublished)_eventPublisher.Events.First();
            Check.That(evt.Content).IsEqualTo(MessageContent);
        }

        [TestMethod]
        public void WhenRepublishMessageThenRaiseMessageRepublished()
        {
            Given(new MessagePublished(MessageId, Author, MessageContent))
            .When(o => o.Republish(_eventPublisher, Republisher))
            .ThenHas(new MessageRepublished(MessageId, Republisher));
        }

        [TestMethod]
        public void WhenRepublishMyOwnMessageThenDoNotRaiseMessageRepublished()
        {
            Given(new MessagePublished(MessageId, Author, MessageContent))
            .When(o => o.Republish(_eventPublisher, Author))
            .ThenNothing();
        }

        [TestMethod]
        public void WhenRepublishTwoTimesSameMessageThenDoNotRaiseMessageRepublished()
        {
            Given(new MessagePublished(MessageId, Author, MessageContent))
                .And(new MessageRepublished(MessageId, Republisher))
            .When(o => o.Republish(_eventPublisher, Republisher))
            .ThenNothing();
        }

        [TestMethod]
        public void WhenReplyThenRaiseReplyMessagePublished()
        {
            Given(new MessagePublished(MessageId, Author, MessageContent))
            .When(o => o.Reply(_eventPublisher, Replier, ReplyContent))
            .ThenHasEvent<ReplyMessagePublished>(evt =>
            {
                Check.That(evt.ParentId).IsEqualTo(MessageId);
                Check.That(evt.ReplyContent).IsEqualTo(ReplyContent);
                Check.That(evt.Replier).IsEqualTo(Replier);
                Check.That(evt.ReplyId).IsNotEqualTo(MessageId);
            });
        }

        [TestMethod]
        public void WhenDeleteThenRaiseMessageDeleted()
        {
            Given(new MessagePublished(MessageId, Author, MessageContent))
            .When(o => o.Delete(_eventPublisher, Author))
            .ThenHasOnly(new MessageDeleted(MessageId));
        }

        [TestMethod]
        public void WhenDeleteBySomeoneElseThanAuthorThenDoNotRaiseMessageDeleted()
        {
            Given(new MessagePublished(MessageId, Author, MessageContent))
            .When(o => o.Delete(_eventPublisher, new UserId("clement@mix-it.fr")))
            .ThenNothing();
        }

        [TestMethod]
        public void GivenIsRepublishedWhenDeleteByRepublisherThenDoNotRaiseMessageDeleted()
        {
            Given(new MessagePublished(MessageId, Author, MessageContent))
                .And(new MessageRepublished(MessageId, Republisher))
            .When(o => o.Delete(_eventPublisher, Republisher))
            .ThenNothing();
        }

        [TestMethod]
        public void GiveDeletedMessageWhenDeleteThenNothing()
        {
            Given(new MessagePublished(MessageId, Author, MessageContent))
                .And(new MessageDeleted(MessageId))
            .When(o => o.Delete(_eventPublisher, Author))
            .ThenNothing();
        }

        [TestMethod]
        public void GivenReplyMessageWhenGetIdHasReplayMessageId()
        {
            var replyMessageId = MessageId.Generate();
            var message = CreateMessage(new ReplyMessagePublished(replyMessageId, Replier, ReplyContent, MessageId));

            Check.That(message.GetId()).IsEqualTo(replyMessageId);
        }

        [TestMethod]
        public void GivenADeletedMessageWhenReplyThenDoNotRaiseMessageDeleted()
        {
            Given(new MessagePublished(MessageId, Author, MessageContent))
                .And(new MessageDeleted(MessageId))
            .When(o => o.Reply(_eventPublisher, Replier, ReplyContent))
            .ThenNothing();
        }

        [TestMethod]
        public void GivenDeletedMessageWhenRepublishThenDoNotRaiseMessageRepublished()
        {
            Given(new MessagePublished(MessageId, Author, MessageContent))
                .And(new MessageDeleted(MessageId))
            .When(o => o.Republish(_eventPublisher, Republisher))
            .ThenNothing();
        }

        private Message CreateMessage(params IDomainEvent[] events)
        {
            return new Message(events);
        }

        private GivenFactory Given(IDomainEvent evt)
        {
            return new GivenFactory(evt, _eventPublisher);
        }

        private class GivenFactory
        {
            private readonly IList<IDomainEvent> _events = new List<IDomainEvent>();
            private readonly EventPublisherFake _eventPublisherFake;

            public GivenFactory(IDomainEvent evt, EventPublisherFake eventPublisherFake)
            {
                _events.Add(evt);
                _eventPublisherFake = eventPublisherFake;
            }

            public GivenFactory And(IDomainEvent evt)
            {
                _events.Add(evt);

                return this;
            }

            public ThenFactory When(Action<Message> when)
            {
                var message = new Message(_events);
                when(message);

                return new ThenFactory(_eventPublisherFake);
            }

            public class ThenFactory
            {
                private readonly EventPublisherFake _eventPublisherFake;

                public ThenFactory(EventPublisherFake eventPublisherFake)
                {
                    _eventPublisherFake = eventPublisherFake;
                }

                public void ThenHas(IDomainEvent domainEvent)
                {
                    Check.That(_eventPublisherFake.Events).Contains(domainEvent);
                }

                public void ThenNothing()
                {
                    Check.That(_eventPublisherFake.Events).IsEmpty();
                }

                public void ThenHasEvent<TEvent>(Action<TEvent> then)
                {
                    var evt = _eventPublisherFake.Events.OfType<TEvent>().First();
                    then(evt);
                }

                public void ThenHasOnly(IDomainEvent domainEvent)
                {
                    Check.That(_eventPublisherFake.Events).ContainsExactly(domainEvent);
                }
            }
        }
    }
}