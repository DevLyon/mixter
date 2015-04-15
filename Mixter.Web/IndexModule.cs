using Mixter.Domain;
using Mixter.Domain.Core.Messages;
using Mixter.Domain.Identity;
using Nancy;
using Nancy.ModelBinding;

namespace Mixter.Web
{
    public class IndexModule : NancyModule
    {
        private readonly ITimelineMessagesRepository _timelineRepository;
        private readonly IEventPublisher _eventPublisher; 

        public IndexModule(ITimelineMessagesRepository timelineRepository, IEventPublisher eventPublisher)
        {
            _timelineRepository = timelineRepository;
            _eventPublisher = eventPublisher;
            
            Get["/"] = parameters => View["index"];
            Post["/api/messages"] = PostMessageOfUser;
            Get["/api/{userId}/messages"] = GetMessageByUserId;
        }

        private object PostMessageOfUser(object arg)
        {
            var message = this.Bind<NancyMessage>();

            if (message.UserId != null)
            {
                Message.Publish(_eventPublisher, new UserId(message.UserId), message.Content);
                return true; 
            }

            return false;
        }

        private object GetMessageByUserId(dynamic arg)
        {
            var userId = new UserId(arg.UserId);
            var messages = _timelineRepository.GetMessagesOfUser(userId);
            return Negotiate.WithModel(messages);
        }
    }

    internal struct NancyMessage
    {
        public string UserId { get; set; }
        public string Content { get; set; }
    }
}
