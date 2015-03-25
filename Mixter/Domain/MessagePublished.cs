namespace Mixter.Domain
{
    public struct MessagePublished : IDomainEvent
    {
        public MessageId Id { get; private set; }
        
        public UserId UserId { get; private set; }

        public string Message { get; private set; }

        public MessagePublished(MessageId id, UserId userId, string message)
            : this()
        {
            Message = message;
            Id = id;
            UserId = userId;
        }
    }
}