using System.Collections.Generic;
using Mixter.Domain.Messages.Handlers;
using Mixter.Domain.Subscriptions.Handlers;

namespace Mixter.Infrastructure
{
    public class EventHandlersGenerator
    {
        private readonly EventsDatabase _eventsDatabase;
        private readonly TimelineMessagesRepository _timelineMessagesRepository;

        public EventHandlersGenerator(EventsDatabase eventsDatabase, TimelineMessagesRepository timelineMessagesRepository)
        {
            _eventsDatabase = eventsDatabase;
            _timelineMessagesRepository = timelineMessagesRepository;
        }

        public IEnumerable<IEventHandler> Generate(EventPublisher eventPublisher)
        {
            yield return new MessagePublishedHandler();
            yield return new AddMessageOnAuthorTimeline(_timelineMessagesRepository);
            yield return new NotifyFollowerOfFolloweeMessage(new SubscriptionRepository(_eventsDatabase), eventPublisher);
            yield return new AddMessageOnFollowerTimeline(_eventsDatabase, _timelineMessagesRepository);
        }
    }
}
