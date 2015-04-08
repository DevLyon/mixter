using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mixter.Domain.Core.Messages;
using Mixter.Domain.Core.Messages.Events;
using Mixter.Domain.Core.Subscriptions;
using Mixter.Domain.Core.Subscriptions.Events;
using Mixter.Domain.Core.Subscriptions.Handlers;
using Mixter.Domain.Identity;
using Mixter.Infrastructure;
using Mixter.Infrastructure.Repositories;
using Mixter.Infrastructure.Tests.Infrastructure;
using NFluent;

namespace Mixter.Domain.Tests.Core.Messages
{
    [TestClass]
    public class NotifyFollowerOfFolloweeMessageTest
    {
        private static readonly UserId Followee = new UserId("followee@mixit.fr");

        private NotifyFollowerOfFolloweeMessage _handler;
        private EventPublisherFake _eventPublisher;
        private EventsStore _store;
        private FollowersRepository _followersRepository;
        private SubscriptionsesRepository _subscriptionsesRepository;

        [TestInitialize]
        public void Initialize()
        {
            _store = new EventsStore();
            _eventPublisher = new EventPublisherFake();
            _followersRepository = new FollowersRepository();
            _subscriptionsesRepository = new SubscriptionsesRepository(_store);
            var messagesRepository = new MessagesRepository(_store);
            _handler = new NotifyFollowerOfFolloweeMessage(_followersRepository, messagesRepository, _eventPublisher, _subscriptionsesRepository);
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
        public void WhenReplyMessagePublishedByFolloweeThenRaiseFolloweeMessagePublished()
        {
            var follower = new UserId("follower@mixit.fr");
            AddFollower(follower);
            var messagePublished = PublishMessage(new UserId("author@mixit.fr"), "Hello");
            var replyMessagePublished = new ReplyMessagePublished(MessageId.Generate(), Followee, "Hello", messagePublished.Id);

            _handler.Handle(replyMessagePublished);

            Check.That(_eventPublisher.Events)
                .Contains(new FolloweeMessagePublished(new SubscriptionId(follower, Followee), replyMessagePublished.ReplyId));
        }

        [TestMethod]
        public void WhenMessageRepublishedByFolloweeThenRaiseFolloweeMessagePublished()
        {
            var follower = new UserId("follower@mixit.fr");
            AddFollower(follower);
            var author = new UserId("author@mixit.fr");
            var messagePublished = PublishMessage(author, "Hello");
            var messageRepublished = new MessageRepublished(messagePublished.Id, Followee);

            _handler.Handle(messageRepublished);

            Check.That(_eventPublisher.Events)
                .Contains(new FolloweeMessagePublished(new SubscriptionId(follower, Followee), messagePublished.Id));
        }

        private MessagePublished PublishMessage(UserId author, string content)
        {
            var messageId = MessageId.Generate();
            var messagePublished = new MessagePublished(messageId, author, content);
            _store.Store(messagePublished);

            return messagePublished;
        }

        private void AddFollower(UserId follower)
        {
            _followersRepository.Save(new FollowerProjection(Followee, follower));
            _store.Store(new UserFollowed(new SubscriptionId(follower, Followee)));
        }
    }
}