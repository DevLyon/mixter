using Mixter.Domain.Identity.Events;

namespace Mixter.Domain.Identity
{
    [Projection]
    public struct SessionProjection
    {
        public SessionId SessionId { get; private set; }

        public UserId UserId { get; private set; }

        public SessionState SessionState { get; private set; }

        public SessionProjection(UserConnected evt)
            : this(evt.SessionId, evt.UserId, SessionState.Enabled)
        {
        }

        public SessionProjection(UserDisconnected evt)
            : this(evt.SessionId, evt.UserId, SessionState.Disabled)
        {
        }

        public SessionProjection(SessionId sessionId, UserId userId, SessionState sessionState)
            : this()
        {
            SessionId = sessionId;
            UserId = userId;
            SessionState = sessionState;
        }
    }
}
