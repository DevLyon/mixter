using Mixter.Domain;
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

            var sessionsRepository = new SessionsRepository();

            var eventPublisher = new EventPublisher();
            var eventPublisherWithStorage = new EventPublisherWithStorage(eventsStore, eventPublisher);
            eventPublisher.Subscribe(new SessionHandler(sessionsRepository));

            container.Register<IEventPublisher>(eventPublisherWithStorage);
        }
    }
}