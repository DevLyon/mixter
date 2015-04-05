using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mixter.Domain.Core.Messages;
using Mixter.Domain.Core.Messages.Events;
using Mixter.Domain.Core.Messages.Handlers;
using Mixter.Domain.Core.Subscriptions;
using Mixter.Domain.Core.Subscriptions.Events;
using Mixter.Domain.Identity;
using Mixter.Infrastructure;
using Mixter.Infrastructure.Repositories;
using NFluent;

namespace Mixter.Tests.Domain.Core.Messages
{
    [TestClass]
    public class UpdateTimelineTest
    {
        private const string Content = "Hello";

        private static readonly UserId Author = new UserId("author@mixit.fr");
        private static readonly MessageId MessageId = MessageId.Generate();

        private TimelineMessagesRepository _repository;
        private UpdateTimeline _handler;
        private EventsStore _store;

        [TestInitialize]
        public void Initialize()
        {
            _store = new EventsStore();
            _repository = new TimelineMessagesRepository();
            _handler = new UpdateTimeline(_repository, new MessagesRepository(_store));
        }

        [TestMethod]
        public void WhenHandleMessagePublishedThenSaveTimelineMessageProjectionForAuthor()
        {
            _handler.Handle(new MessagePublished(MessageId, Author, Content));

            Check.That(_repository.GetMessagesOfUser(Author))
                 .ContainsExactly(new TimelineMessageProjection(Author, Author, Content, MessageId));
        }

        [TestMethod]
        public void WhenHandleMessageRepliedThenSaveTimelineMessageProjectionForReplier()
        {
            var parentMessageId = MessageId.Generate();
            var replier = new UserId("author@mixit.fr");
            _handler.Handle(new ReplyMessagePublished(MessageId, replier, Content, parentMessageId));

            Check.That(_repository.GetMessagesOfUser(replier))
                 .ContainsExactly(new TimelineMessageProjection(replier, replier, Content, MessageId));
        }

        [TestMethod]
        public void GivenMessagePublishedByFolloweeWhenHandleFolloweeMessagePublishedThenSaveTimelineMessageProjection()
        {
            var followee = Author;
            _store.Store(new MessagePublished(MessageId, followee, Content));
            var follower = new UserId("owner@mixit.fr");
            _handler.Handle(new FolloweeMessagePublished(new SubscriptionId(follower, followee), MessageId));

            Check.That(_repository.GetMessagesOfUser(follower))
                 .ContainsExactly(new TimelineMessageProjection(follower, followee, Content, MessageId));
        }

        [TestMethod]
        public void GivenMessageRepublishedByFolloweeWhenHandleFolloweeMessagePublishedThenSaveTimelineMessageProjectionWithOriginalAuthor()
        {
            _store.Store(new MessagePublished(MessageId, Author, Content));
            var followee = new UserId("followee@mixit.fr");
            _store.Store(new MessageRepublished(MessageId, followee));
            var follower = new UserId("owner@mixit.fr");
            _handler.Handle(new FolloweeMessagePublished(new SubscriptionId(follower, followee), MessageId));

            Check.That(_repository.GetMessagesOfUser(follower))
                 .ContainsExactly(new TimelineMessageProjection(follower, Author, Content, MessageId));
        }
    }
}
