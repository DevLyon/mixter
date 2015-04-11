using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mixter.Domain.Core.Messages;
using Mixter.Domain.Core.Messages.Events;
using Mixter.Domain.Core.Subscriptions;
using Mixter.Domain.Core.Subscriptions.Events;
using Mixter.Domain.Core.Subscriptions.Handlers;
using Mixter.Domain.Identity;
using Mixter.Infrastructure;
using Mixter.Infrastructure.Tests;
using NFluent;

namespace Mixter.Domain.Tests.Core.Subscriptions.Handlers
{
    [TestClass]
    public class NotifyFollowerOfFolloweeMessageTest
    {
        private static readonly UserId Followee = new UserId("followee@mixit.fr");

        private NotifyFollowerOfFolloweeMessage _handler;
        private EventPublisherFake _eventPublisher;
        private EventsStore _store;
        private FollowersRepository _followersRepository;
        private SubscriptionsRepository _subscriptionsRepository;

        [TestInitialize]
        public void Initialize()
        {
            _store = new EventsStore();
            _eventPublisher = new EventPublisherFake();
            _followersRepository = new FollowersRepository();
            _subscriptionsRepository = new SubscriptionsRepository(_store);
            var messagesRepository = new MessagesRepository(_store);
            _handler = new NotifyFollowerOfFolloweeMessage(_followersRepository, messagesRepository, _eventPublisher, _subscriptionsRepository);
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

        private void AddFollower(UserId follower)
        {
            _followersRepository.Save(new FollowerProjection(Followee, follower));
            _store.Store(new UserFollowed(new SubscriptionId(follower, Followee)));
        }
    }
}