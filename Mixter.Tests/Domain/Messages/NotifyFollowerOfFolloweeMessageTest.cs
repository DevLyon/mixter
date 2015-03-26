using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mixter.Domain;
using Mixter.Domain.Messages;
using Mixter.Domain.Messages.Events;
using Mixter.Domain.Messages.Handlers;
using Mixter.Domain.Subscriptions;
using Mixter.Domain.Subscriptions.Events;
using Mixter.Infrastructure;
using Mixter.Infrastructure.Repositories;
using Mixter.Tests.Infrastructure;
using NFluent;

namespace Mixter.Tests.Domain.Messages
{
    [TestClass]
    public class NotifyFollowerOfFolloweeMessageTest
    {
        private static readonly UserId Followee = new UserId("followee@mixit.fr");

        private ISubscriptionRepository _subscriptionRepository;
        private NotifyFollowerOfFolloweeMessage _handler;
        private EventPublisherFake _eventPublisher;
        private EventsDatabase _database;

        [TestInitialize]
        public void Initialize()
        {
            _database = new EventsDatabase();
            _subscriptionRepository = new SubscriptionsRepository(_database);
            _eventPublisher = new EventPublisherFake();
            _handler = new NotifyFollowerOfFolloweeMessage(_subscriptionRepository, _eventPublisher);
        }

        [TestMethod]
        public void WhenMessagePublishedByFolloweeThenRaiseFollowerMessagePublished()
        {
            var follower = new UserId("follower@mixit.fr");
            AddFollower(follower);
            var messagePublished = new MessagePublished(MessageId.Generate(), Followee, "content");

            _handler.Handle(messagePublished);

            Check.That(_eventPublisher.Events).Contains(new FollowerMessagePublished(new SubscriptionId(follower, Followee), messagePublished.Id));
        }

        [TestMethod]
        public void WhenReplyMessagePublishedByFolloweeThenRaiseFollowerMessagePublished()
        {
            var follower = new UserId("follower@mixit.fr");
            AddFollower(follower);
            var replyMessagePublished = new ReplyMessagePublished(MessageId.Generate(), Followee, "Hello", MessageId.Generate());

            _handler.Handle(replyMessagePublished);

            Check.That(_eventPublisher.Events).Contains(new FollowerMessagePublished(new SubscriptionId(follower, Followee), replyMessagePublished.ReplyId));
        }

        private void AddFollower(UserId follower)
        {
            _database.Store(new UserFollowed(new SubscriptionId(follower, Followee)));
        }
    }
}