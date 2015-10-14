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
        public static SessionId LogIn(IEventPublisher eventPublisher, UserId userId)
        {
            var id = SessionId.Generate();
            eventPublisher.Publish(new UserConnected(id, userId, DateTime.Now));

            return id;
        }

        [Command]
        public void Logout(IEventPublisher eventPublisher)
        {
            if (_projection.IsDisconnected)
            {
                return;
            }

            eventPublisher.Publish(new UserDisconnected(_projection.Id, _projection.UserId));
        }

        [Projection]
        private class DecisionProjection : DecisionProjectionBase
        {
            public SessionId Id { get; private set; }

            public UserId UserId { get; private set; }

            public bool IsDisconnected { get; private set; }

            public DecisionProjection()
            {
                AddHandler<UserConnected>(When);
                AddHandler<UserDisconnected>(When);
            }

            private void When(UserDisconnected evt)
            {
                IsDisconnected = true;
            }

            private void When(UserConnected evt)
            {
                Id = evt.SessionId;
                UserId = evt.UserId;
            }
        }
    }
}
