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
        private readonly SubscriptionsRepository _repository;

        public SubscriptionsRepositoryTest()
        {
            _eventsStore = new EventsStore();
            _repository = new SubscriptionsRepository(_eventsStore);
        }

        [Fact]
        public void GivenUserFollowedWhenGetSubscriptionThenReturnSubscriptionAggregate()
        {
            _eventsStore.Store(new UserFollowed(SubscriptionId));

            var subscription = _repository.GetSubscription(SubscriptionId);

            var eventPublisher = new EventPublisherFake();
            subscription.Unfollow(eventPublisher);
            Check.That(eventPublisher.Events).ContainsExactly(new UserUnfollowed(SubscriptionId));
        }

        [Fact]
        public void GivenNotEventWhenGetSubscriptionThenThrowUnknownSubscription()
        {
            Check.ThatCode(() => _repository.GetSubscription(SubscriptionId)).Throws<UnknownSubscription>();
        }
    }
}
