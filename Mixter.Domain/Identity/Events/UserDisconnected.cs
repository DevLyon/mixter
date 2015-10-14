namespace Mixter.Domain.Identity.Events
{
    public struct UserDisconnected : IDomainEvent
    {
        public SessionId SessionId { get; private set; }

        public UserId UserId { get; private set; }

        public UserDisconnected(SessionId sessionId, UserId userId)
            : this()
        {
            SessionId = sessionId;
            UserId = userId;
        }

        public object GetAggregateId()
        {
            return SessionId;
        }
    }
}
