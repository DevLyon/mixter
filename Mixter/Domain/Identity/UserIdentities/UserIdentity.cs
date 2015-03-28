using Mixter.Domain.Identity.UserIdentities.Events;

namespace Mixter.Domain.Identity.UserIdentities
{
    public class UserIdentity
    {
        private readonly DecisionProjection _projection = new DecisionProjection();

        public UserIdentity(params IDomainEvent[] events)
        {
            foreach (var @event in events)
            {
                _projection.Apply(@event);
            }
        }

        public static void Register(IEventPublisher eventPublisher, UserId userId)
        {
            eventPublisher.Publish(new UserRegistered(userId));
        }

        public void Log(IEventPublisher eventPublisher)
        {
            Session.LogUser(eventPublisher, _projection.Id);
        }

        private class DecisionProjection : DecisionProjectionBase
        {
            public UserId Id { get; private set; }

            public DecisionProjection()
            {
                AddHandler<UserRegistered>(When);
            }

            private void When(UserRegistered evt)
            {
                Id = evt.UserId;
            }
        }
    }
}