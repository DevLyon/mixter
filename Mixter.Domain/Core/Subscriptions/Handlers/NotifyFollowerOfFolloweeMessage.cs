using Mixter.Domain.Core.Messages;
using Mixter.Domain.Core.Messages.Events;
using Mixter.Domain.Identity;

namespace Mixter.Domain.Core.Subscriptions.Handlers
{
    public class NotifyFollowerOfFolloweeMessage : 
        IEventHandler<MessagePublished>,
        IEventHandler<ReplyMessagePublished>,
        IEventHandler<MessageRepublished>
    {
        private readonly IFollowersRepository _followersRepository;
        private readonly IMessagesRepository _messagesRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly ISubscriptionsRepository _subscriptionsRepository;

        public NotifyFollowerOfFolloweeMessage(
            IFollowersRepository followersRepository, 
            IMessagesRepository messagesRepository,
            IEventPublisher eventPublisher, 
            ISubscriptionsRepository subscriptionsRepository)
        {
            _followersRepository = followersRepository;
            _messagesRepository = messagesRepository;
            _eventPublisher = eventPublisher;
            _subscriptionsRepository = subscriptionsRepository;
        }

        public void Handle(MessagePublished evt)
        {
            NotifyAllFollowers(evt.Author, evt.Id);
        }

        public void Handle(MessageRepublished evt)
        {
            NotifyAllFollowers(evt.Republisher, evt.Id);
        }

        public void Handle(ReplyMessagePublished evt)
        {
        }

        private void NotifyAllFollowers(UserId followee, MessageId messageId)
        {
            foreach (var follower in _followersRepository.GetFollowers(followee))
            {
                var subscription = _subscriptionsRepository.Get(new SubscriptionId(follower, followee));
                subscription.NotifyFollower(_eventPublisher, messageId);
            }
        }
    }
}