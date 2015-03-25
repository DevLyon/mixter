using Mixter.Domain.Subscriptions;
using Mixter.Infrastructure;

namespace Mixter.Domain.Messages
{
    public class TimelineMessageHandler : IEventHandler<MessagePublished>
    {
        private readonly ITimelineMessagesRepository _timelineMessagesRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;

        public TimelineMessageHandler(ITimelineMessagesRepository timelineMessagesRepository, ISubscriptionRepository subscriptionRepository)
        {
            _timelineMessagesRepository = timelineMessagesRepository;
            _subscriptionRepository = subscriptionRepository;
        }

        public void Handle(MessagePublished evt)
        {
            AddMessageInTimeline(evt.Author, evt.Author, evt.Content, evt.Id);

            var followers = _subscriptionRepository.GetFollowers(evt.Author);
            foreach (var follower in followers)
            {
                AddMessageInTimeline(follower, evt.Author, evt.Content, evt.Id);
            }
        }

        private void AddMessageInTimeline(UserId owner, UserId author, string content, MessageId messageId)
        {
            var authorMessage = new TimelineMessage(owner, author, content, messageId);
            _timelineMessagesRepository.Save(authorMessage);
        }
    }
}