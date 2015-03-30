using Mixter.Domain.Core.Messages.Events;
using Mixter.Infrastructure;

namespace Mixter.Domain.Core.Messages.Handlers
{
    public class AddMessageOnAuthorTimeline : 
        IEventHandler<MessagePublished>,
        IEventHandler<ReplyMessagePublished>
    {
        private readonly ITimelineMessagesRepository _timelineMessagesRepository;

        public AddMessageOnAuthorTimeline(ITimelineMessagesRepository timelineMessagesRepository)
        {
            _timelineMessagesRepository = timelineMessagesRepository;
        }

        public void Handle(MessagePublished evt)
        {
            var authorMessage = new TimelineMessageProjection(evt.Author, evt);
            _timelineMessagesRepository.Save(authorMessage);
        }

        public void Handle(ReplyMessagePublished evt)
        {
            var authorMessage = new TimelineMessageProjection(evt.Replier, evt);
            _timelineMessagesRepository.Save(authorMessage);
        }
    }
}