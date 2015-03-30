using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mixter.Domain.Core.Messages;
using Mixter.Domain.Core.Messages.Events;
using Mixter.Domain.Core.Messages.Handlers;
using Mixter.Domain.Identity;
using NFluent;

namespace Mixter.Tests.Domain.Core.Messages
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

            Check.That(_timelineMessagesRepositoryFake.Messages.Single()).IsEqualTo(new TimelineMessageProjection(new UserId("author"), messagePublished));
        }

        [TestMethod]
        public void WhenHandleReplyMessagePublishedThenReplyMessageIsSaved()
        {
            var replyMessagePublished = new ReplyMessagePublished(MessageId.Generate(), new UserId("author"), "content", MessageId.Generate());
            _handler.Handle(replyMessagePublished);

            Check.That(_timelineMessagesRepositoryFake.Messages.Single()).IsEqualTo(new TimelineMessageProjection(new UserId("author"), replyMessagePublished));
        }

        private class TimelineMessagesRepositoryFake : ITimelineMessagesRepository
        {
            private readonly IList<TimelineMessageProjection> _messages = new List<TimelineMessageProjection>();

            public IEnumerable<TimelineMessageProjection> Messages
            {
                get { return _messages; }
            }

            public void Save(TimelineMessageProjection messageProjection)
            {
                _messages.Add(messageProjection);
            }

            public IEnumerable<TimelineMessageProjection> GetMessagesOfUser(UserId userId)
            {
                throw new NotImplementedException();
            }
        }
    }
}
