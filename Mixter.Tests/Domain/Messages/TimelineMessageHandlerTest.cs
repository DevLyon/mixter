using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mixter.Domain;
using Mixter.Domain.Messages;
using Mixter.Domain.Subscriptions;
using Mixter.Infrastructure;
using Mixter.Tests.Infrastructure;
using NFluent;

namespace Mixter.Tests.Domain.Messages
{
    [TestClass]
    public class TimelineMessageHandlerTest
    {
        private static readonly UserId Followee = new UserId("followee@mixit.fr");

        private TimelineMessagesRepositoryFake _timelineMessagesRepositoryFake;
        private ISubscriptionRepository _subscriptionRepository;
        private TimelineMessageHandler _handler;
        private EventPublisherFake _eventPublisher;
        private EventsDatabase _database;

        [TestInitialize]
        public void Initialize()
        {
            _database = new EventsDatabase();
            _timelineMessagesRepositoryFake = new TimelineMessagesRepositoryFake();
            _subscriptionRepository = new SubscriptionRepository(_database);
            _eventPublisher = new EventPublisherFake();
            _handler = new TimelineMessageHandler(_timelineMessagesRepositoryFake, _subscriptionRepository, _eventPublisher);
        }

        [TestMethod]
        public void WhenHandleMessagePublishedThenMessageIsSaved()
        {
            var author = new UserId("author");
            var messageId = MessageId.Generate();
            const string content = "content";
            _handler.Handle(new MessagePublished(messageId, author, content));

            Check.That(_timelineMessagesRepositoryFake.Messages.Single()).IsEqualTo(new TimelineMessage(author, author, content, messageId));
        }

        [TestMethod]
        public void WhenMessagePublishedByFolloweeThenRaiseFollowerMessagePublished()
        {
            var follower = new UserId("follower@mixit.fr");
            AddFollower(follower);

            var messageId = MessageId.Generate();
            const string content = "content";
            _handler.Handle(new MessagePublished(messageId, Followee, content));

            Check.That(_eventPublisher.Events).Contains(new FollowerMessagePublished(new SubscriptionId(follower, Followee), messageId));
        }

        private void AddFollower(UserId follower)
        {
            _database.Store(new UserFollowed(new SubscriptionId(follower, Followee)));
        }

        private class TimelineMessagesRepositoryFake : ITimelineMessagesRepository
        {
            private readonly IList<TimelineMessage> _messages = new List<TimelineMessage>();

            public IEnumerable<TimelineMessage> Messages
            {
                get { return _messages; }
            }

            public void Save(TimelineMessage message)
            {
                _messages.Add(message);
            }

            public IEnumerable<TimelineMessage> GetMessagesOfUser(UserId userId)
            {
                throw new NotImplementedException();
            }
        }
    }
}
