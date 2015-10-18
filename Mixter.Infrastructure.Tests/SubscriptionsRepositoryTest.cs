using Mixter.Domain.Core.Subscriptions;
using Mixter.Domain.Core.Subscriptions.Events;
using Mixter.Domain.Identity;
using Mixter.Domain.Tests;
using NFluent;
using Xunit;

namespace Mixter.Infrastructure.Tests
{
    public class SubscriptionsRepositoryTest
    {
        private static readonly SubscriptionId SubscriptionId = new SubscriptionId(new UserId("follower@mix-it.fr"), new UserId("followee@mix-it.fr"));

        private readonly EventsStore _eventsStore;
        private readonly SubscriptionsRepository _subscriptionsRepository;
        private readonly FollowersRepository _followersRepository;

        public SubscriptionsRepositoryTest()
        {
            _eventsStore = new EventsStore();
            _followersRepository = new FollowersRepository();
            _subscriptionsRepository = new SubscriptionsRepository(_eventsStore, _followersRepository);
        }

        [Fact]
        public void GivenUserFollowedWhenGetSubscriptionThenReturnSubscriptionAggregate()
        {
            _eventsStore.Store(new UserFollowed(SubscriptionId));

            var subscription = _subscriptionsRepository.GetSubscription(SubscriptionId);

            var eventPublisher = new EventPublisherFake();
            subscription.Unfollow(eventPublisher);
            Check.That(eventPublisher.Events).ContainsExactly(new UserUnfollowed(SubscriptionId));
        }

        [Fact]
        public void GivenNotEventWhenGetSubscriptionThenThrowUnknownSubscription()
        {
            Check.ThatCode(() => _subscriptionsRepository.GetSubscription(SubscriptionId)).Throws<UnknownSubscription>();
        }

        [Fact]
        public void WhenGetSubscriptionsOfUserThenReturnAllSubscriptionAggregatesOfUser()
        {
            var followee = new UserId("followee@mix-it.fr");
            var follower1 = new UserId("follower1@mix-it.fr");
            var follower2 = new UserId("follower2@mix-it.fr");
            _followersRepository.Save(new FollowerProjection(followee, follower1));
            _followersRepository.Save(new FollowerProjection(followee, follower2));
            _eventsStore.Store(new UserFollowed(new SubscriptionId(follower1, followee)));
            _eventsStore.Store(new UserFollowed(new SubscriptionId(follower2, followee)));

            var subscriptions = _subscriptionsRepository.GetSubscriptionsOfUser(followee);

            Check.That(subscriptions).HasSize(2);
        }
    }
}
