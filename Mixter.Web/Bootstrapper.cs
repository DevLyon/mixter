using System;
using System.IO;
using Mixter.Domain;
using Mixter.Domain.Core.Messages;
using Mixter.Infrastructure;
using Nancy;
using Nancy.Conventions;
using Nancy.Helpers;
using Nancy.Responses;
using Nancy.TinyIoc;

namespace Mixter.Web
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        private const string PublicDirectory = @"../../../public/";

        protected override void ConfigureConventions(NancyConventions conventions)
        {
            base.ConfigureConventions(conventions);

            conventions.StaticContentsConventions.Clear();
            conventions.StaticContentsConventions.Add(AddDirectory(PublicDirectory));

            conventions.ViewLocationConventions.Add((viewName, model, context) => string.Concat(PublicDirectory, viewName));
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);
            container.Register<EventsStore>();
            container.Register<ITimelineMessagesRepository, TimelineMessagesRepository>();
            container.Register<IMessagesRepository, MessagesRepository>();
            
            var eventHandlersGenerator = new EventHandlersGenerator(container.Resolve<EventsStore>(), container.Resolve<ITimelineMessagesRepository>());
            container.Register<IEventPublisher>(new EventPublisherWithStorage(container.Resolve<EventsStore>(), new EventPublisher(eventHandlersGenerator.Generate)));
        }

        private static Func<NancyContext, string, Response> AddDirectory(string contentPath)
        {
            GenericFileResponse.SafePaths.Add(Path.GetFullPath(contentPath));

            return (ctx, root) => BuildContentDelegate(ctx, contentPath);
        }

        private static Response BuildContentDelegate(NancyContext nancyContext, string contentPath)
        {
            var path = HttpUtility.UrlDecode(nancyContext.Request.Path);
            if (string.IsNullOrEmpty(Path.GetFileName(path)))
            {
                return null;
            }

            var fileName = Path.GetFullPath(contentPath + path);

            return File.Exists(fileName) 
                       ? new GenericFileResponse(fileName, nancyContext) 
                       : null;
        }
    }
}