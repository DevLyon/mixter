using Mixter.Domain.Core.Messages.Events;

namespace Mixter.Domain.Core.Messages.Handlers
{
    public class UpdateTimeline : IEventHandler<TimelineMessagePublished>
    {
        private readonly ITimelineMessagesRepository _repository;

        public UpdateTimeline(ITimelineMessagesRepository repository)
        {
            _repository = repository;
        }

        public void Handle(TimelineMessagePublished evt)
        {
            var projection = new TimelineMessageProjection(evt.Id.Owner, evt.Author, evt.Content, evt.Id.MessageId);
            _repository.Save(projection);
        }
    }
}
