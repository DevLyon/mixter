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
        private readonly EventsStore _eventsStore;
        private readonly ITimelineMessagesRepository _timelineMessagesRepository;

        public EventHandlersGenerator(EventsStore eventsStore, ITimelineMessagesRepository timelineMessagesRepository)
        {
            _eventsStore = eventsStore;
            _timelineMessagesRepository = timelineMessagesRepository;
        }

        public IEnumerable<IEventHandler> Generate(IEventPublisher eventPublisher)
        {
            var messagesRepository = new MessagesRepository(_eventsStore);
            var subscriptionsesRepository = new SubscriptionsesRepository(_eventsStore);
            var followersRepository = new FollowersRepository();
            var sessionsRepository = new SessionsRepository();

            yield return new MessagePublishedHandler();
            yield return new NotifyFollowerOfFolloweeMessage(followersRepository, messagesRepository, eventPublisher, subscriptionsesRepository);
            yield return new SessionHandler(sessionsRepository);
            yield return new UpdateTimeline(_timelineMessagesRepository, messagesRepository);
            yield return new UpdateFollowers(followersRepository);
        }
    }
}
