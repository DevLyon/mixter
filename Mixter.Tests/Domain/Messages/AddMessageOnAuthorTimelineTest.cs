using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mixter.Domain;
using Mixter.Domain.Messages;
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
            var author = new UserId("author");
            var messageId = MessageId.Generate();
            const string content = "content";
            _handler.Handle(new MessagePublished(messageId, author, content));

            Check.That(_timelineMessagesRepositoryFake.Messages.Single()).IsEqualTo(new TimelineMessage(author, author, content, messageId));
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
