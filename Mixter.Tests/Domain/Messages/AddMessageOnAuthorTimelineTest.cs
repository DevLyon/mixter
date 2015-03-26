using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mixter.Domain;
using Mixter.Domain.Messages;
using Mixter.Domain.Messages.Events;
using Mixter.Domain.Messages.Handlers;
using NFluent;

namespace Mixter.Tests.Domain.Messages
{
    [TestClass]
    public class AddMessageOnAuthorTimelineTest
    {
        private TimelineMessagesRepositoryFake _timelineMessagesRepositoryFake;
        private AddMessageOnAuthorTimeline _handler;

        [TestInitialize]
        public void Initialize()
        {
            _timelineMessagesRepositoryFake = new TimelineMessagesRepositoryFake();
            _handler = new AddMessageOnAuthorTimeline(_timelineMessagesRepositoryFake);
        }

        [TestMethod]
        public void WhenHandleMessagePublishedThenMessageIsSaved()
        {
            var messagePublished = new MessagePublished(MessageId.Generate(), new UserId("author"), "content");
            _handler.Handle(messagePublished);

            Check.That(_timelineMessagesRepositoryFake.Messages.Single()).IsEqualTo(new TimelineMessage(new UserId("author"), messagePublished));
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
