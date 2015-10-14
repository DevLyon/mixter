using System;

namespace Mixter.Domain.Identity.Events
{
    [Event]
    public struct UserConnected : IDomainEvent
    {
        public SessionId SessionId { get; private set; }

        public UserId UserId { get; private set; }

        public DateTime ConnectedAt { get; private set; }

        public UserConnected(SessionId sessionId, UserId userId, DateTime connectedAt)
            : this()
        {
            SessionId = sessionId;
            UserId = userId;
            ConnectedAt = connectedAt;
        }

        public object GetAggregateId()
        {
            return SessionId;
        }
    }
}
