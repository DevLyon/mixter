namespace Mixter.Domain
{
    public struct MessagePublished : IDomainEvent
    {
        public MessageId Id { get; private set; }
        
        public UserId Creator { get; private set; }

        public string Content { get; private set; }

        public MessagePublished(MessageId id, UserId creator, string content)
            : this()
        {
            Content = content;
            Id = id;
            Creator = creator;
        }
    }
}