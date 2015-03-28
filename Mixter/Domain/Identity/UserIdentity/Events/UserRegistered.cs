using Mixter.Domain.Core;

namespace Mixter.Domain.Identity.UserIdentity.Events
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