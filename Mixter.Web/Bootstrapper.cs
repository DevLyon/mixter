using Mixter.Domain;
using Mixter.Domain.Core.Messages;
using Mixter.Domain.Core.Messages.Handlers;
using Mixter.Domain.Identity;
using Mixter.Infrastructure;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Responses.Negotiation;
using Nancy.TinyIoc;

namespace Mixter.Web
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override NancyInternalConfiguration InternalConfiguration
        {
            get
            {
                return NancyInternalConfiguration.WithOverrides(config => {
                    config.StatusCodeHandlers = new[] { typeof(NotFoundStatusCodeHandler), typeof(ExceptionStatusCodeHandler) };
                    config.ResponseProcessors = new[] { typeof(JsonProcessor) };
                });
            }
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            var eventsStore = new EventsStore();

            var sessionsRepository = new SessionsRepository(eventsStore);
            var timelineMessageRepository = new TimelineMessageRepository();

            var eventPublisher = new EventPublisher();
            var eventPublisherWithStorage = new EventPublisherWithStorage(eventsStore, eventPublisher);
            eventPublisher.Subscribe(new SessionHandler(sessionsRepository));
            eventPublisher.Subscribe(new UpdateTimeline(timelineMessageRepository));

            container.Register<IEventPublisher>(eventPublisherWithStorage);
            container.Register<IUserIdentitiesRepository>(new UserIdentitiesRepository(eventsStore));
            container.Register<ISessionsRepository>(sessionsRepository);
            container.Register<ITimelineMessageRepository>(timelineMessageRepository);
        }
    }
}