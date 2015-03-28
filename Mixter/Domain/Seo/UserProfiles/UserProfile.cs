using Mixter.Domain.Core;
using Mixter.Domain.Seo.UserProfiles.Events;

namespace Mixter.Domain.Seo.UserProfiles
{
    public class UserProfile
    {
        private readonly DecisionProjection _projection = new DecisionProjection();

        public UserProfile(params IDomainEvent[] events)
        {
            foreach (var @event in events)
            {
                _projection.Apply(@event);
            }
        }

        public static void Create(IEventPublisher eventPublisher, UserId userId, string firstName, string lastName)
        {
            var id = new UserProfileId(userId);
            eventPublisher.Publish(new UserProfileCreated(id, userId, firstName, lastName));
        }

        public void UpdateDescription(IEventPublisher eventPublisher, string newFirstName, string newLastName)
        {
            if (_projection.FirstName == newFirstName && _projection.LastName == newLastName)
            {
                return;
            }

            eventPublisher.Publish(new UserDescriptionUpdated(_projection.Id, newFirstName, newLastName));
        }

        private class DecisionProjection : DecisionProjectionBase
        {
            public UserProfileId Id { get; private set; }

            public string FirstName { get; private set; }

            public string LastName { get; private set; }

            public DecisionProjection()
            {
                AddHandler<UserProfileCreated>(When);
            }

            private void When(UserProfileCreated evt)
            {
                Id = evt.Id;
                FirstName = evt.FirstName;
                LastName = evt.LastName;
            }
        }
    }
}