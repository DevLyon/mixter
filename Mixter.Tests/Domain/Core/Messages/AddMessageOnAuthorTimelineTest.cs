using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mixter.Domain.Core.Messages;
using Mixter.Domain.Core.Messages.Events;
using Mixter.Domain.Core.Messages.Handlers;
using Mixter.Domain.Identity;
using Mixter.Tests.Infrastructure;
using NFluent;

namespace Mixter.Tests.Domain.Core.Messages
{
    [TestClass]
    public class AddMessageOnAuthorTimelineTest
    {
        private AddMessageOnAuthorTimeline _handler;
        private EventPublisherFake _eventPublisher;

        [TestInitialize]
        public void Initialize()
        {
            _eventPublisher = new EventPublisherFake();
            _handler = new AddMessageOnAuthorTimeline(_eventPublisher);
        }

        [TestMethod]
        public void WhenHandleMessagePublishedThenRaiseTimelineMessagePublished()
        {
            var messagePublished = new MessagePublished(MessageId.Generate(), new UserId("author"), "content");

            _handler.Handle(messagePublished);

            var timelineMessageId = new TimelineMessageId(messagePublished.Author, messagePublished.Id);
            var expectedEvent = new TimelineMessagePublished(timelineMessageId, messagePublished.Author, messagePublished.Content);
            Check.That(_eventPublisher.Events)
                .ContainsExactly(expectedEvent);
        }

        [TestMethod]
        public void WhenHandleReplyMessagePublishedThenReplyMessageIsSaved()
        {
            var replyMessagePublished = new ReplyMessagePublished(MessageId.Generate(), new UserId("author"), "content", MessageId.Generate());
            _handler.Handle(replyMessagePublished);

            var timelineMessageId = new TimelineMessageId(replyMessagePublished.Replier, replyMessagePublished.ReplyId);
            var expectedEvent = new TimelineMessagePublished(timelineMessageId, replyMessagePublished.Replier, replyMessagePublished.ReplyContent);
            Check.That(_eventPublisher.Events)
                .ContainsExactly(expectedEvent);
        }
    }
}
