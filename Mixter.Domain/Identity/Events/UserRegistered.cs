namespace Mixter.Domain.Identity.Events
{
    public struct UserRegistered : IDomainEvent
    {
        public UserId UserId { get; private set; }

        public UserRegistered(UserId userId)
            : this()
        {
            UserId = userId;
        }

        public object GetAggregateId()
        {
            return UserId;
        }
    }
}
