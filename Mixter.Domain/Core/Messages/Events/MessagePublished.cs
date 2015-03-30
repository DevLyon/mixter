using Mixter.Domain.Identity;

namespace Mixter.Domain.Core.Messages.Events
{
    public struct MessagePublished : IDomainEvent
    {
        public MessageId Id { get; private set; }
        
        public UserId Author { get; private set; }

        public string Content { get; private set; }

        public MessagePublished(MessageId id, UserId author, string content)
            : this()
        {
            Content = content;
            Id = id;
            Author = author;
        }

        public object GetAggregateId()
        {
            return Id;
        }
    }
}