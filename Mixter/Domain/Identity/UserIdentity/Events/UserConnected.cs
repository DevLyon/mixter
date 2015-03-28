using System;
using Mixter.Domain.Core;

namespace Mixter.Domain.Identity.UserIdentity.Events
{
    public struct UserConnected : IDomainEvent
    {
        public UserId UserId { get; private set; }

        public DateTime ConnectedAt { get; private set; }

        public UserConnected(UserId userId, DateTime connectedAt)
            : this()
        {
            UserId = userId;
            ConnectedAt = connectedAt;
        }

        public object GetAggregateId()
        {
            return UserId;
        }
    }
}