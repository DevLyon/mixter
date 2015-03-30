using Mixter.Domain.Core.Messages.Events;

namespace Mixter.Domain.Core.Messages.Handlers
{
    public class AddMessageOnAuthorTimeline : 
        IEventHandler<MessagePublished>,
        IEventHandler<ReplyMessagePublished>
    {
        private readonly IEventPublisher _eventPublisher;

        public AddMessageOnAuthorTimeline(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        public void Handle(MessagePublished evt)
        {
            TimelineMessage.Publish(_eventPublisher, evt.Author, evt.Author, evt.Content, evt.Id);
        }

        public void Handle(ReplyMessagePublished evt)
        {
            TimelineMessage.Publish(_eventPublisher, evt.Replier, evt.Replier, evt.ReplyContent, evt.ReplyId);
        }
    }
}