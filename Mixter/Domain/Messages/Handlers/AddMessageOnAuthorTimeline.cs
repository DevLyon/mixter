using Mixter.Domain.Messages.Events;
using Mixter.Infrastructure;

namespace Mixter.Domain.Messages.Handlers
{
    public class AddMessageOnAuthorTimeline : IEventHandler<MessagePublished>
    {
        private readonly ITimelineMessagesRepository _timelineMessagesRepository;

        public AddMessageOnAuthorTimeline(ITimelineMessagesRepository timelineMessagesRepository)
        {
            _timelineMessagesRepository = timelineMessagesRepository;
        }

        public void Handle(MessagePublished evt)
        {
            var authorMessage = new TimelineMessage(evt.Author, evt.Author, evt.Content, evt.Id);
            _timelineMessagesRepository.Save(authorMessage);
        }
    }
}