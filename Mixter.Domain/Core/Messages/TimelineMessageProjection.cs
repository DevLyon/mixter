using Mixter.Domain.Identity;

namespace Mixter.Domain.Core.Messages
{
    [Projection]
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
    }
}
