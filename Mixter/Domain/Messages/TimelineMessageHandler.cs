namespace Mixter.Domain.Messages
{
    public class TimelineMessageHandler
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