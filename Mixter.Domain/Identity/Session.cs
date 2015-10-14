using System;
using Mixter.Domain.Identity.Events;

namespace Mixter.Domain.Identity
{
    [Aggregate]
    public class Session
    {
        private readonly DecisionProjection _projection = new DecisionProjection();

        public Session(params IDomainEvent[] events)
        {
            foreach (var @event in events)
            {
                _projection.Apply(@event);
            }
        }

        [Command]
        public static void LogIn(IEventPublisher eventPublisher, UserId userId)
        {
            var id = SessionId.Generate();
            eventPublisher.Publish(new UserConnected(id, userId, DateTime.Now));
        }

        [Command]
        public void Logout(IEventPublisher eventPublisher)
        {
            eventPublisher.Publish(new UserDisconnected(_projection.Id, _projection.UserId));
        }

        [Projection]
        private class DecisionProjection
        {
            public SessionId Id { get; private set; }

            public UserId UserId { get; private set; }

            public void Apply(IDomainEvent @event)
            {
                When((dynamic)@event);
            }

            private void When(UserConnected evt)
            {
                Id = evt.SessionId;
                UserId = evt.UserId;
            }
        }
    }
}
