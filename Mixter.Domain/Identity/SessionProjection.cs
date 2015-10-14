namespace Mixter.Domain.Identity
{
    [Projection]
    public struct SessionProjection
    {
        public SessionId SessionId { get; private set; }

        public UserId UserId { get; private set; }

        public SessionState SessionState { get; private set; }

        public SessionProjection(SessionId sessionId, UserId userId, SessionState sessionState)
            : this()
        {
            SessionId = sessionId;
            UserId = userId;
            SessionState = sessionState;
        }
    }
}
