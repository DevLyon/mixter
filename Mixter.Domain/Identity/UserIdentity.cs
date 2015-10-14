using Mixter.Domain.Identity.Events;

namespace Mixter.Domain.Identity
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

        public void LogIn(IEventPublisher eventPublisher)
        {
            Session.LogIn(eventPublisher, _projection.Id);
        }

        private class DecisionProjection
        {
            public UserId Id { get; private set; }

            public void Apply(IDomainEvent @event)
            {
                When((dynamic)@event);
            }

            private void When(UserRegistered evt)
            {
                Id = evt.UserId;
            }
        }
    }
}
