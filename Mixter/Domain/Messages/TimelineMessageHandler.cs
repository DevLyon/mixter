using Mixter.Domain.Subscriptions;
using Mixter.Infrastructure;

namespace Mixter.Domain.Messages
{
    public class TimelineMessageHandler : IEventHandler<MessagePublished>
    {
        private readonly ITimelineMessagesRepository _timelineMessagesRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IEventPublisher _eventPublisher;

        public TimelineMessageHandler(ITimelineMessagesRepository timelineMessagesRepository, ISubscriptionRepository subscriptionRepository, IEventPublisher eventPublisher)
        {
            _timelineMessagesRepository = timelineMessagesRepository;
            _subscriptionRepository = subscriptionRepository;
            _eventPublisher = eventPublisher;
        }

        public void Handle(MessagePublished evt)
        {
            AddMessageInTimeline(evt.Author, evt.Author, evt.Content, evt.Id);

            var followers = _subscriptionRepository.GetFollowers(evt.Author);
            foreach (var follower in followers)
            {
                follower.NotifyFollower(_eventPublisher, evt.Id);
            }
        }

        private void AddMessageInTimeline(UserId owner, UserId author, string content, MessageId messageId)
        {
            var authorMessage = new TimelineMessage(owner, author, content, messageId);
            _timelineMessagesRepository.Save(authorMessage);
        }
    }
}