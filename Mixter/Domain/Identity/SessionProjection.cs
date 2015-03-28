using Mixter.Domain.Identity.Events;

namespace Mixter.Domain.Identity
{
    public struct SessionProjection
    {
        public SessionId SessionId { get; private set; }

        public UserId UserId { get; private set; }

        public SessionProjection(UserConnected evt)
            : this(evt.SessionId, evt.UserId)
        {
        }

        public SessionProjection(SessionId sessionId, UserId userId) 
            : this()
        {
            SessionId = sessionId;
            UserId = userId;
        }
    }
}