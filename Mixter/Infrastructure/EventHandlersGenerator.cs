using System.Collections.Generic;
using Mixter.Domain;
using Mixter.Domain.Messages;
using Mixter.Domain.Messages.Handlers;
using Mixter.Domain.Subscriptions.Handlers;
using Mixter.Infrastructure.Repositories;

namespace Mixter.Infrastructure
{
    public class EventHandlersGenerator
    {
        private readonly EventsDatabase _eventsDatabase;
        private readonly ITimelineMessagesRepository _timelineMessagesRepository;

        public EventHandlersGenerator(EventsDatabase eventsDatabase, ITimelineMessagesRepository timelineMessagesRepository)
        {
            _eventsDatabase = eventsDatabase;
            _timelineMessagesRepository = timelineMessagesRepository;
        }

        public IEnumerable<IEventHandler> Generate(IEventPublisher eventPublisher)
        {
            yield return new MessagePublishedHandler();
            yield return new AddMessageOnAuthorTimeline(_timelineMessagesRepository);
            yield return new NotifyFollowerOfFolloweeMessage(new SubscriptionRepository(_eventsDatabase), eventPublisher);
            yield return new AddMessageOnFollowerTimeline(_eventsDatabase, _timelineMessagesRepository);
        }
    }
}
