using System;
using Mixter.Domain.Identity.Events;

namespace Mixter.Domain.Identity
{
    public class Session
    {
        public static void LogIn(IEventPublisher eventPublisher, UserId userId)
        {
            var id = SessionId.Generate();
            eventPublisher.Publish(new UserConnected(id, userId, DateTime.Now));
        }
    }
}
