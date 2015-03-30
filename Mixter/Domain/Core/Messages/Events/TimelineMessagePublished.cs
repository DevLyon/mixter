using Mixter.Domain.Identity;

namespace Mixter.Domain.Core.Messages.Events
{
    public struct TimelineMessagePublished : IDomainEvent
    {
        public TimelineMessageId Id { get; private set; }

        public UserId Author { get; private set; }

        public string Content { get; private set; }

        public TimelineMessagePublished(TimelineMessageId id, UserId author, string content)
            : this()
        {
            Id = id;
            Author = author;
            Content = content;
        }

        public object GetAggregateId()
        {
            return Id;
        }
    }
}