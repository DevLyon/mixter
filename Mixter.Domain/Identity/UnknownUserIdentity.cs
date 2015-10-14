namespace Mixter.Domain.Identity
{
    public class UnknownUserIdentity : DomainException
    {
        public UserId UserId { get; private set; }

        public UnknownUserIdentity(UserId userId)
            : base("Unknown user identity with id " + userId)
        {
            UserId = userId;
        }
    }
}