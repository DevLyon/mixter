using System.Linq;
using Mixter.Domain.Core.Messages;
using Mixter.Domain.Core.Messages.Events;
using Mixter.Domain.Core.Subscriptions.Events;
using Mixter.Infrastructure;

namespace Mixter.Domain.Core.Subscriptions.Handlers
{
    public class AddMessageOnFollowerTimeline : IEventHandler<FollowerMessagePublished>
    {
        private readonly EventsDatabase _database;
        private readonly ITimelineMessagesRepository _timelineMessageRepository;

        public AddMessageOnFollowerTimeline(EventsDatabase database, ITimelineMessagesRepository timelineMessageRepository)
        {
            _database = database;
            _timelineMessageRepository = timelineMessageRepository;
        }

        public void Handle(FollowerMessagePublished evt)
        {
            var messagePublished = _database.GetEventsOfAggregate(evt.MessageId).OfType<MessagePublished>().First();
            var ownerId = evt.SubscriptionId.Follower;

            _timelineMessageRepository.Save(new TimelineMessageProjection(ownerId, messagePublished));
        }
    }
}
