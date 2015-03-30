namespace Mixter.Domain.Seo.UserProfiles.Events
{
    public struct UserDescriptionUpdated : IDomainEvent
    {
        public UserProfileId Id { get; private set; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public UserDescriptionUpdated(UserProfileId id, string firstName, string lastName)
            : this()
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
        }

        public object GetAggregateId()
        {
            return Id;
        }
    }
}