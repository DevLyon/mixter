// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimelineMessageHandlerTest.cs" company="">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------



using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mixter.Domain;
using Mixter.Domain.Messages;
using Mixter.Domain.Subscriptions;
using Mixter.Tests.Infrastructure;
using NFluent;

namespace Mixter.Tests.Domain.Messages
{
    [TestClass]
    public class TimelineMessageHandlerTest
    {
        private TimelineMessagesRepositoryFake _timelineMessagesRepositoryFake;
        private SubscriptionRepositoryFake _subscriptionRepositoryFake;
        private TimelineMessageHandler _handler;

        [TestInitialize]
        public void Initialize()
        {
            _timelineMessagesRepositoryFake = new TimelineMessagesRepositoryFake();
            _subscriptionRepositoryFake = new SubscriptionRepositoryFake();
            _handler = new TimelineMessageHandler(_timelineMessagesRepositoryFake, _subscriptionRepositoryFake);
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
        public void WhenMessagePublishedByFolloweeThenAddMessageInFollowerTimeline()
        {
            var follower = new UserId("follower@mixit.fr");
            var followee = new UserId("followee@mixit.fr");
            _subscriptionRepositoryFake.AddFollower(follower);

            var messageId = MessageId.Generate();
            const string content = "content";
            _handler.Handle(new MessagePublished(messageId, followee, content));

            Check.That(_timelineMessagesRepositoryFake.Messages).Contains(new TimelineMessage(follower, followee, content, messageId));
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

    public class SubscriptionRepositoryFake : ISubscriptionRepository
    {
        private readonly IList<UserId> _followers = new List<UserId>();

        public void AddFollower(UserId follower)
        {
            _followers.Add(follower);
        }

        public IEnumerable<UserId> GetFollowers(UserId userId)
        {
            return _followers;
        }
    }
}
