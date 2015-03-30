using Mixter.Domain.Identity;

namespace Mixter.Domain.Core.Messages.Events
{
    public struct ReplyMessagePublished : IDomainEvent
    {
        public MessageId ReplyId { get; private set; }

        public UserId Replier { get; private set; }

        public string ReplyContent { get; private set; }
        
        public MessageId ParentId { get; private set; }

        public ReplyMessagePublished(
            MessageId replyId, 
            UserId replier, 
            string replyContent, 
            MessageId parentId)
            : this()
        {
            ReplyId = replyId;
            Replier = replier;
            ReplyContent = replyContent;
            ParentId = parentId;
        }

        public object GetAggregateId()
        {
            return ReplyId;
        }
    }
}