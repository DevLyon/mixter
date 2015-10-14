namespace Mixter.Domain.Identity
{
    public class UnknownSession : DomainException
    {
        public SessionId SessionId { get; private set; }

        public UnknownSession(SessionId sessionId)
            : base("Unknown session with id " + sessionId)
        {
            SessionId = sessionId;
        }
    }
}