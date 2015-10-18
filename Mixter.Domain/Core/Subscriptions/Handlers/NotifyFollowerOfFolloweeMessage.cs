using Mixter.Domain.Core.Messages.Events;

namespace Mixter.Domain.Core.Subscriptions.Handlers
{
    public class NotifyFollowerOfFolloweeMessage
    {
        private readonly IFollowersRepository _followersRepository;
        private readonly ISubscriptionsRepository _subscriptionsRepository;
        private readonly IEventPublisher _eventPublisher;

        public NotifyFollowerOfFolloweeMessage(IFollowersRepository followersRepository, ISubscriptionsRepository subscriptionsRepository, IEventPublisher eventPublisher)
        {
            _followersRepository = followersRepository;
            _subscriptionsRepository = subscriptionsRepository;
            _eventPublisher = eventPublisher;
        }

        public void Handle(MessageQuacked evt)
        {
            foreach (var follower in _followersRepository.GetFollowers(evt.Author))
            {
                var subscription = _subscriptionsRepository.GetSubscription(new SubscriptionId(follower, evt.Author));
                subscription.NotifyFollower(_eventPublisher, evt.Id);
            }
        }
    }
}
