using System.Linq;
using Mixter.Domain.Messages;
using Mixter.Domain.Messages.Events;
using Mixter.Domain.Subscriptions.Events;
using Mixter.Infrastructure;

namespace Mixter.Domain.Subscriptions.Handlers
{
    public class AddMessageOnFollowerTimeline : IEventHandler<FollowerMessagePublished>
    {
        private readonly EventsDatabase _database;
        private readonly TimelineMessagesRepository _timelineMessageRepository;

        public AddMessageOnFollowerTimeline(EventsDatabase database, TimelineMessagesRepository timelineMessageRepository)
        {
            _database = database;
            _timelineMessageRepository = timelineMessageRepository;
        }

        public void Handle(FollowerMessagePublished evt)
        {
            var messagePublished = _database.GetEventsOfAggregate(evt.MessageId).OfType<MessagePublished>().First();
            var ownerId = evt.SubscriptionId.Follower;

            _timelineMessageRepository.Save(new TimelineMessage(ownerId, messagePublished));
        }
    }
}
