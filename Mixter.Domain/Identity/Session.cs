using System;
using Mixter.Domain.Identity.Events;

namespace Mixter.Domain.Identity
{
    [Aggregate]
    public class Session
    {
        [Command]
        public static void LogIn(IEventPublisher eventPublisher, UserId userId)
        {
            var id = SessionId.Generate();
            eventPublisher.Publish(new UserConnected(id, userId, DateTime.Now));
        }
    }
}
