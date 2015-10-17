using System;
using Mixter.Domain;
using Mixter.Domain.Core.Messages;
using Mixter.Domain.Identity;
using Nancy;
using Nancy.ModelBinding;

namespace Mixter.Web
{
    public class CoreApi : NancyModule
    {
        public CoreApi(IEventPublisher eventPublisher, ITimelineMessageRepository timelineMessageRepository)
            : base("/api/core")
        {
            Post("/messages/quack", _ => Execute(eventPublisher, this.Bind<QuackMessage>()));
            Get("/timelineMessages/{author}", _ => Execute(eventPublisher, timelineMessageRepository, (string)_.author));
        }

        private dynamic Execute(IEventPublisher eventPublisher, QuackMessage command)
        {
            var messageId = Message.Quack(eventPublisher, new UserId(command.Author), command.Content);

            return Negotiate.WithStatusCode(HttpStatusCode.Created).WithModel(new
            {
                Id = messageId,
                Url = "/api/core/messages/" + Uri.EscapeUriString(messageId.ToString())
            });
        }

        private dynamic Execute(IEventPublisher eventPublisher, ITimelineMessageRepository timelineMessageRepository, string author)
        {
            var messages = timelineMessageRepository.GetMessagesOfUser(new UserId(author));

            return Negotiate.WithStatusCode(HttpStatusCode.OK).WithModel(messages);
        }

        private class QuackMessage
        {
            public string Author { get; set; }

            public string Content { get; set; }
        }
    }
}
