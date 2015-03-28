using Mixter.Domain.Core;

namespace Mixter.Domain.Seo.UserProfiles.Events
{
    public struct UserProfileCreated : IDomainEvent
    {
        public UserProfileId Id { get; private set; }

        public UserId UserId { get; private set; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public UserProfileCreated(UserProfileId id, UserId userId, string firstName, string lastName)
            : this()
        {
            Id = id;
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
        }

        public object GetAggregateId()
        {
            return Id;
        }
    }
}