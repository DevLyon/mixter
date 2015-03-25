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
            var authorMessage = new TimelineMessage(evt.Author, evt.Author, evt.Content, evt.Id);
            _timelineMessagesRepository.Save(authorMessage);                

            var followers = _subscriptionRepository.GetFollowers(evt.Author);
            foreach (var follower in followers)
            {
                var message = new TimelineMessage(follower, evt.Author, evt.Content, evt.Id);
                _timelineMessagesRepository.Save(message);                
            }
        }
    }
}