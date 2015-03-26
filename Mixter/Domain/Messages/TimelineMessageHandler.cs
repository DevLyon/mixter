using Mixter.Domain.Subscriptions;
using Mixter.Infrastructure;

namespace Mixter.Domain.Messages
{
    public class TimelineMessageHandler : IEventHandler<MessagePublished>
    {
        private readonly NotifyFollowerOfFolloweeMessage _notifyFollowerOfFolloweeMessage;
        private readonly AddMessageOnAuthorTimeline _addMessageOnAuthorTimeline;

        public TimelineMessageHandler(ITimelineMessagesRepository timelineMessagesRepository, ISubscriptionRepository subscriptionRepository, IEventPublisher eventPublisher)
        {
            _notifyFollowerOfFolloweeMessage = new NotifyFollowerOfFolloweeMessage(subscriptionRepository, eventPublisher);
            _addMessageOnAuthorTimeline = new AddMessageOnAuthorTimeline(timelineMessagesRepository);
        }

        public void Handle(MessagePublished evt)
        {
            _addMessageOnAuthorTimeline.Handle(evt);
            _notifyFollowerOfFolloweeMessage.Handle(evt);
        }
    }
}