using Mixter.Domain.Core.Messages;
using Mixter.Domain.Core.Messages.Events;
using Mixter.Domain.Identity;

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
            NotifyAllFollowers(evt.Author, evt.Id);
        }

        public void Handle(MessageRequacked evt)
        {
            NotifyAllFollowers(evt.Requacker, evt.Id);
        }

        private void NotifyAllFollowers(UserId followee, MessageId messageId)
        {
            foreach (var follower in _followersRepository.GetFollowers(followee))
            {
                var subscription = _subscriptionsRepository.GetSubscription(new SubscriptionId(follower, followee));
                subscription.NotifyFollower(_eventPublisher, messageId);
            }
        }
    }
}
