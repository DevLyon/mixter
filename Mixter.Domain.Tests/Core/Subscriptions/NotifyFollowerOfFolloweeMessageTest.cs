using Mixter.Domain.Core.Messages;
using Mixter.Domain.Core.Messages.Events;
using Mixter.Domain.Core.Subscriptions;
using Mixter.Domain.Core.Subscriptions.Events;
using Mixter.Domain.Core.Subscriptions.Handlers;
using Mixter.Domain.Identity;
using Mixter.Infrastructure;
using NFluent;
using Xunit;

namespace Mixter.Domain.Tests.Core.Subscriptions
{
    public class NotifyFollowerOfFolloweeMessageTest
    {
        private static readonly SubscriptionId SubscriptionId = new SubscriptionId(new UserId("follower@mix-it.fr"), new UserId("followee@mix-it.fr"));

        private readonly EventsStore _eventsStore;
        private readonly NotifyFollowerOfFolloweeMessage _handler;
        private readonly FollowersRepository _followersRepository;
        private readonly EventPublisherFake _eventPublisher;

        public NotifyFollowerOfFolloweeMessageTest()
        {
            _eventsStore = new EventsStore();
            _followersRepository = new FollowersRepository();
            var subscriptionsRepository = new SubscriptionsRepository(_eventsStore, _followersRepository);
            _eventPublisher = new EventPublisherFake();
            _handler = new NotifyFollowerOfFolloweeMessage(_followersRepository, subscriptionsRepository, _eventPublisher);
        }

        [Fact]
        public void GivenFollowerWhenMessageQuackedByFolloweeThenRaiseFolloweeMessageQuacked()
        {
            AddFollower(SubscriptionId);
            var messageQuacked = new MessageQuacked(MessageId.Generate(), SubscriptionId.Followee, "hello");

            _handler.Handle(messageQuacked);

            Check.That(_eventPublisher.Events).Contains(new FolloweeMessageQuacked(SubscriptionId, messageQuacked.Id));
        }

        [Fact]
        public void WhenMessageRequackedByFolloweeThenRaiseFolloweeMessageQuacked()
        {
            AddFollower(SubscriptionId);
            var author = new UserId("author@mixit.fr");
            var messageQuacked = new MessageQuacked(MessageId.Generate(), author, "hello");
            var messageRequacked = new MessageRequacked(messageQuacked.Id, SubscriptionId.Followee);

            _handler.Handle(messageRequacked);

            Check.That(_eventPublisher.Events).Contains(new FolloweeMessageQuacked(SubscriptionId, messageQuacked.Id));
        }

        private void AddFollower(SubscriptionId subscriptionId)
        {
            _followersRepository.Save(new FollowerProjection(SubscriptionId.Followee, subscriptionId.Follower));
            _eventsStore.Store(new UserFollowed(subscriptionId));
        }
    }
}
