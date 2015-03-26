using Mixter.Domain.Messages.Events;

namespace Mixter.Domain.Messages
{
    public struct TimelineMessage
    {
        public UserId OwnerId { get; private set; }
        
        public UserId AuthorId { get; private set; }
        
        public string Content { get; private set; }

        public MessageId MessageId { get; private set; }

        public TimelineMessage(UserId ownerId, UserId authorId, string content, MessageId messageId) 
            : this()
        {
            OwnerId = ownerId;
            AuthorId = authorId;
            Content = content;
            MessageId = messageId;
        }

        public TimelineMessage(UserId ownerId, MessagePublished evt)
            : this(ownerId, evt.Author, evt.Content, evt.Id)
        {
        }
    }
}