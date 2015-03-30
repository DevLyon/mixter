using System.Collections.Generic;
using Mixter.Domain;
using Mixter.Domain.Core.Messages;
using Mixter.Domain.Core.Messages.Handlers;
using Mixter.Domain.Core.Subscriptions.Handlers;
using Mixter.Domain.Identity;
using Mixter.Infrastructure;
using Mixter.Infrastructure.Repositories;

namespace Mixter
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
            yield return new AddMessageOnAuthorTimeline(eventPublisher);
            yield return new NotifyFollowerOfFolloweeMessage(new FollowersRepository(), eventPublisher, _eventsDatabase);
            yield return new SessionHandler(new SessionsRepository());
            yield return new UpdateTimeline(_timelineMessagesRepository);
            yield return new UpdateFollowers(new FollowersRepository());
        }
    }
}
