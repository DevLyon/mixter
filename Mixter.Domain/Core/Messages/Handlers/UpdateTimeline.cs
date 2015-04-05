using Mixter.Domain.Core.Messages.Events;
using Mixter.Domain.Core.Subscriptions.Events;
using Mixter.Domain.Identity;

namespace Mixter.Domain.Core.Messages.Handlers
{
    public class UpdateTimeline : IEventHandler<MessagePublished>, IEventHandler<ReplyMessagePublished>, IEventHandler<FolloweeMessagePublished>
    {
        private readonly ITimelineMessagesRepository _timelineMessagesRepository;
        private readonly IMessagesRepository _messagesRepository;

        public UpdateTimeline(ITimelineMessagesRepository timelineMessagesRepository, IMessagesRepository messagesRepository)
        {
            _timelineMessagesRepository = timelineMessagesRepository;
            _messagesRepository = messagesRepository;
        }

        public void Handle(FolloweeMessagePublished evt)
        {
            var ownerId = evt.SubscriptionId.Follower;
            var messageDescription = _messagesRepository.GetDescription(evt.MessageId);

            Save(ownerId, messageDescription.Author, messageDescription.Content, evt.MessageId);
        }

        public void Handle(MessagePublished evt)
        {
            var ownerId = evt.Author;
            var authorId = evt.Author;
            var content = evt.Content;
            var messageId = evt.Id;

            Save(ownerId, authorId, content, messageId);
        }

        public void Handle(ReplyMessagePublished evt)
        {
            var ownerId = evt.Replier;
            var authorId = evt.Replier;
            var content = evt.ReplyContent;
            var messageId = evt.ReplyId;

            Save(ownerId, authorId, content, messageId);
        }

        private void Save(UserId ownerId, UserId authorId, string content, MessageId messageId)
        {
            var projection = new TimelineMessageProjection(ownerId, authorId, content, messageId);
            _timelineMessagesRepository.Save(projection);
        }
    }
}
