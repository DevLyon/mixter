using Mixter.Domain.Subscriptions;
using Mixter.Infrastructure;

namespace Mixter.Domain.Messages
{
    public class NotifyFollowerOfFolloweeMessage : IEventHandler<MessagePublished>
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IEventPublisher _eventPublisher;

        public NotifyFollowerOfFolloweeMessage(ISubscriptionRepository subscriptionRepository, IEventPublisher eventPublisher)
        {
            _subscriptionRepository = subscriptionRepository;
            _eventPublisher = eventPublisher;
        }

        public void Handle(MessagePublished evt)
        {
            var followers = _subscriptionRepository.GetFollowers(evt.Author);
            foreach (var follower in followers)
            {
                follower.NotifyFollower(_eventPublisher, evt.Id);
            }
        }
    }
}