using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mixter.Domain.Core.Messages;
using Mixter.Domain.Core.Messages.Events;
using Mixter.Domain.Core.Messages.Handlers;
using Mixter.Domain.Core.Subscriptions;
using Mixter.Domain.Core.Subscriptions.Events;
using Mixter.Domain.Identity;
using Mixter.Infrastructure.Repositories;
using NFluent;

namespace Mixter.Tests.Domain.Core.Messages
{
    [TestClass]
    public class UpdateTimelineTest
    {
        [TestMethod]
        public void WhenHandleTimelineMessagePublishedThenSaveTimelineMessageProjection()
        {
            var repository = new TimelineMessagesRepository();
            var handler = new UpdateTimeline(repository);

            var owner = new UserId("owner@mixit.fr");
            var messageId = MessageId.Generate();
            var author = new UserId("author@mixit.fr");
            var content = "Hello";
            handler.Handle(new TimelineMessagePublished(new TimelineMessageId(owner, messageId), author, content));

            Check.That(repository.GetMessagesOfUser(owner))
                 .ContainsExactly(new TimelineMessageProjection(owner, author, content, messageId));
        }

        [TestMethod]
        public void WhenHandleMessagePublishedThenSaveTimelineMessageProjectionForAuthor()
        {
            var repository = new TimelineMessagesRepository();
            var handler = new UpdateTimeline(repository);

            var messageId = MessageId.Generate();
            var author = new UserId("author@mixit.fr");
            var content = "Hello";
            handler.Handle(new MessagePublished(messageId, author, content));

            Check.That(repository.GetMessagesOfUser(author))
                 .ContainsExactly(new TimelineMessageProjection(author, author, content, messageId));
        }

        [TestMethod]
        public void WhenHandleMessageRepliedThenSaveTimelineMessageProjectionForReplier()
        {
            var repository = new TimelineMessagesRepository();
            var handler = new UpdateTimeline(repository);

            var parentMessageId = MessageId.Generate();
            var messageId = MessageId.Generate();
            var replier = new UserId("author@mixit.fr");
            var replyContent = "Hello";
            handler.Handle(new ReplyMessagePublished(messageId, replier, replyContent, parentMessageId));

            Check.That(repository.GetMessagesOfUser(replier))
                 .ContainsExactly(new TimelineMessageProjection(replier, replier, replyContent, messageId));
        }

        [TestMethod]
        public void WhenHandleFolloweeMessagePublishedThenSaveTimelineMessageProjection()
        {
            var repository = new TimelineMessagesRepository();
            var handler = new UpdateTimeline(repository);

            var owner = new UserId("owner@mixit.fr");
            var messageId = MessageId.Generate();
            var author = new UserId("author@mixit.fr");
            var content = "Hello";
            handler.Handle(new FolloweeMessagePublished(new SubscriptionId(owner, author), messageId, content));

            Check.That(repository.GetMessagesOfUser(owner))
                 .ContainsExactly(new TimelineMessageProjection(owner, author, content, messageId));
        }
    }
}
