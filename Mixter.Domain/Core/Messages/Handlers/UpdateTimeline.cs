using Mixter.Domain.Core.Messages.Events;
using Mixter.Domain.Core.Subscriptions.Events;

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

        public void Handle(FolloweeMessagePublished evt)
        {
            var projection = 
                new TimelineMessageProjection(evt.SubscriptionId.Follower, evt.SubscriptionId.Followee, evt.Content, evt.MessageId);
            _repository.Save(projection);
        }

        public void Handle(MessagePublished evt)
        {
            var projection =
                new TimelineMessageProjection(evt.Author, evt.Author, evt.Content, evt.Id);
            _repository.Save(projection);
        }

        public void Handle(ReplyMessagePublished evt)
        {
            var projection =
                new TimelineMessageProjection(evt.Replier, evt.Replier, evt.ReplyContent, evt.ReplyId);
            _repository.Save(projection);
        }
    }
}
