using Mixter.Domain.Core.Messages.Events;
using Mixter.Domain.Core.Subscriptions;
using Mixter.Infrastructure;

namespace Mixter.Domain.Core.Messages.Handlers
{
    public class NotifyFollowerOfFolloweeMessage : 
        IEventHandler<MessagePublished>,
        IEventHandler<ReplyMessagePublished>,
        IEventHandler<MessageRepublished>
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
            NotifyAllFollowers(evt.Author, evt.Id);
        }

        public void Handle(ReplyMessagePublished evt)
        {
            NotifyAllFollowers(evt.Replier, evt.ReplyId);
        }

        public void Handle(MessageRepublished evt)
        {
            NotifyAllFollowers(evt.Republisher, evt.Id);
        }

        private void NotifyAllFollowers(UserId author, MessageId messageId)
        {
            var followers = _subscriptionRepository.GetFollowers(author);
            foreach (var follower in followers)
            {
                follower.NotifyFollower(_eventPublisher, messageId);
            }
        }
    }
}