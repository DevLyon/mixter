namespace Mixter.Domain
{
    public struct MessageRepublished : IDomainEvent
    {
        public MessageId Id { get; private set; }

        public MessageRepublished(MessageId id, UserId userId)
            : this()
        {
            Id = id;
        }
    }
}