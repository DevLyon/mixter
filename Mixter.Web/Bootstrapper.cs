using Mixter.Domain;
using Mixter.Domain.Core.Messages;
using Mixter.Domain.Identity;
using Mixter.Infrastructure;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Responses.Negotiation;
using Nancy.TinyIoc;
using System;

namespace Mixter.Web
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override Func<ITypeCatalog, NancyInternalConfiguration> InternalConfiguration
        {
            get
            {
                return typeCatalog => NancyInternalConfiguration.WithOverrides(config => {
                        config.StatusCodeHandlers = new[] { typeof(NotFoundStatusCodeHandler), typeof(ExceptionStatusCodeHandler) };
                        config.ResponseProcessors = new[] { typeof(JsonProcessor) };
                    })(typeCatalog);
            }
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            var eventsStore = new EventsStore();

            var sessionsRepository = new SessionsRepository(new EventsStore());

            var eventPublisher = new EventPublisher();
            var eventPublisherWithStorage = new EventPublisherWithStorage(eventsStore, eventPublisher);
            eventPublisher.Subscribe(new SessionHandler(sessionsRepository));

            container.Register<IEventPublisher>(eventPublisherWithStorage);
            container.Register<IUserIdentitiesRepository>(new UserIdentitiesRepository(eventsStore));
            container.Register<ISessionsRepository>(sessionsRepository);
            container.Register<ITimelineMessageRepository>(new TimelineMessageRepository());
        }
    }
}