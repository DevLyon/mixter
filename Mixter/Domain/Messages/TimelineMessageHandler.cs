using Mixter.Infrastructure;

namespace Mixter.Domain.Messages
{
    public class TimelineMessageHandler : IEventHandler<MessagePublished>
    {
        private readonly ITimelineMessagesRepository _repository;

        public TimelineMessageHandler(ITimelineMessagesRepository repository)
        {
            _repository = repository;
        }

        public void Handle(MessagePublished evt)
        {
            var message = new TimelineMessage(evt.Author, evt.Author, evt.Content, evt.Id);
            _repository.Save(message);
        }
    }
}