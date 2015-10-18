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
        public CoreApi(IEventPublisher eventPublisher, ITimelineMessageRepository timelineMessageRepository, ISessionsRepository sessionsRepository, IMessagesRepository messagesRepository)
            : base("/api/core")
        {
            Post("/messages/quack", _ => Execute(eventPublisher, this.Bind<QuackMessage>()));
            Delete("/messages/{id}", _ => Execute(eventPublisher, sessionsRepository, messagesRepository, this.Bind<DeleteMessage>().WithMessageId((string)_.id)));
            Get("/timelineMessages/{author}", _ => Execute(timelineMessageRepository, (string)_.author));
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

        private dynamic Execute(IEventPublisher eventPublisher, ISessionsRepository sessionsRepository, IMessagesRepository messagesRepository, DeleteMessage command)
        {
            var sessionId = new SessionId(command.SessionId);

            var deleter = sessionsRepository.GetUserIdOfSession(sessionId);
            if (!deleter.HasValue)
            {
                return Negotiate.WithStatusCode(HttpStatusCode.Forbidden).WithModel("Invalid session");
            }
            
            var messageId = new MessageId(command.MessageId);
            var messageToDeleted = messagesRepository.Get(messageId);

            messageToDeleted.Delete(eventPublisher, deleter.Value);

            return Negotiate.WithStatusCode(HttpStatusCode.OK).WithModel("Message deleted");
        }

        private dynamic Execute(ITimelineMessageRepository timelineMessageRepository, string author)
        {
            var messages = timelineMessageRepository.GetMessagesOfUser(new UserId(author));

            return Negotiate.WithStatusCode(HttpStatusCode.OK).WithModel(messages);
        }

        private class QuackMessage
        {
            public string Author { get; set; }

            public string Content { get; set; }
        }

        private class DeleteMessage
        {
            public string SessionId { get; set; }

            public string MessageId { get; set; }

            public DeleteMessage WithMessageId(string id)
            {
                MessageId = id;

                return this;
            }
        }
    }
}
