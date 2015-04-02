using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mixter.Domain;
using Mixter.Domain.Core.Messages;
using Mixter.Domain.Core.Messages.Events;
using Mixter.Domain.Core.Messages.Handlers;
using Mixter.Domain.Core.Subscriptions;
using Mixter.Domain.Core.Subscriptions.Events;
using Mixter.Domain.Identity;
using Mixter.Infrastructure;
using Mixter.Infrastructure.Repositories;
using Mixter.Tests.Infrastructure;
using NFluent;

namespace Mixter.Tests.Domain.Core.Messages
{
    [TestClass]
    public class NotifyFollowerOfFolloweeMessageTest
    {
        private static readonly UserId Followee = new UserId("followee@mixit.fr");

        private NotifyFollowerOfFolloweeMessage _handler;
        private EventPublisherFake _eventPublisher;
        private EventsDatabase _database;
        private FollowersRepository _followersRepository;
        private SubscriptionsesRepository _subscriptionsesRepository;

        [TestInitialize]
        public void Initialize()
        {
            _database = new EventsDatabase();
            _eventPublisher = new EventPublisherFake();
            _followersRepository = new FollowersRepository();
            _subscriptionsesRepository = new SubscriptionsesRepository(_database);
            _handler = new NotifyFollowerOfFolloweeMessage(_followersRepository, _eventPublisher, _database, _subscriptionsesRepository);
        }

        [TestMethod]
        public void WhenMessagePublishedByFolloweeThenRaiseFolloweeMessagePublished()
        {
            var follower = new UserId("follower@mixit.fr");
            AddFollower(follower);
            var messagePublished = new MessagePublished(MessageId.Generate(), Followee, "content");

            _handler.Handle(messagePublished);

            Check.That(_eventPublisher.Events)
                .Contains(new FolloweeMessagePublished(new SubscriptionId(follower, Followee), messagePublished.Id));
        }

        [TestMethod]
        public void WhenReplyMessagePublishedByFolloweeThenRaiseTimelineMessagePublished()
        {
            var follower = new UserId("follower@mixit.fr");
            AddFollower(follower);
            var messagePublished = PublishMessage(new UserId("author@mixit.fr"), "Hello");
            var replyMessagePublished = new ReplyMessagePublished(MessageId.Generate(), Followee, "Hello", messagePublished.Id);

            _handler.Handle(replyMessagePublished);

            Check.That(_eventPublisher.Events)
                .Contains(new TimelineMessagePublished(new TimelineMessageId(follower, replyMessagePublished.ReplyId), Followee, replyMessagePublished.ReplyContent));
        }

        [TestMethod]
        public void WhenMessageRepublishedByFolloweeThenRaiseTimelineMessageRepublished()
        {
            var follower = new UserId("follower@mixit.fr");
            AddFollower(follower);
            var author = new UserId("author@mixit.fr");
            var messagePublished = PublishMessage(author, "Hello");
            var messageRepublished = new MessageRepublished(messagePublished.Id, Followee);

            _handler.Handle(messageRepublished);

            Check.That(_eventPublisher.Events)
                .Contains(new TimelineMessagePublished(new TimelineMessageId(follower, messagePublished.Id), author, messagePublished.Content));
        }

        private MessagePublished PublishMessage(UserId author, string content)
        {
            var messageId = MessageId.Generate();
            var messagePublished = new MessagePublished(messageId, author, content);
            _database.Store(messagePublished);

            return messagePublished;
        }

        private void AddFollower(UserId follower)
        {
            _followersRepository.Save(new FollowerProjection(Followee, follower));
            _database.Store(new UserFollowed(new SubscriptionId(follower, Followee)));
        }
    }
}