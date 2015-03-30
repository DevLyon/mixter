using Mixter.Domain.Core.Messages.Events;
using Mixter.Domain.Identity;

namespace Mixter.Domain.Core.Messages
{
    public struct TimelineMessageProjection
    {
        public UserId OwnerId { get; private set; }
        
        public UserId AuthorId { get; private set; }
        
        public string Content { get; private set; }

        public MessageId MessageId { get; private set; }

        public TimelineMessageProjection(UserId ownerId, UserId authorId, string content, MessageId messageId) 
            : this()
        {
            OwnerId = ownerId;
            AuthorId = authorId;
            Content = content;
            MessageId = messageId;
        }

        public TimelineMessageProjection(UserId ownerId, MessagePublished evt)
            : this(ownerId, evt.Author, evt.Content, evt.Id)
        {
        }

        public TimelineMessageProjection(UserId ownerId, ReplyMessagePublished evt)
            : this(ownerId, evt.Replier, evt.ReplyContent, evt.ReplyId)
        {
        }
    }
}