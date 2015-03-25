namespace Mixter.Domain
{
    public struct MessagePublished : IDomainEvent
    {
        public MessageId Id { get; private set; }
        
        public UserId Creator { get; private set; }

        public string Message { get; private set; }

        public MessagePublished(MessageId id, UserId creator, string message)
            : this()
        {
            Message = message;
            Id = id;
            Creator = creator;
        }
    }
}