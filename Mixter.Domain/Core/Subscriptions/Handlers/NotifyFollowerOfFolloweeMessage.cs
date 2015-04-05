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
            NotifyAllFollowers(evt.Author, evt.Author, evt.Id, evt.Content);
        }

        public void Handle(ReplyMessagePublished evt)
        {
            NotifyAllFollowers(evt.Replier, evt.Replier, evt.ReplyId, evt.ReplyContent);
        }

        public void Handle(MessageRepublished evt)
        {
            var messageDescription = _messagesRepository.GetDescription(evt.Id);

            NotifyAllFollowers(evt.Republisher, messageDescription.Author, evt.Id, messageDescription.Content);
        }

        private void NotifyAllFollowers(UserId followee, UserId author, MessageId messageId, string content)
        {
            foreach (var follower in _followersRepository.GetFollowers(followee))
            {
                var subscription = _subscriptionsRepository.Get(new SubscriptionId(follower, followee));
                subscription.NotifyFollower(_eventPublisher, messageId, content);
            }
        }
    }
}