using System;
using Mixter.Domain.Identity.UserIdentities.Events;

namespace Mixter.Domain.Identity.UserIdentities
{
    public class Session
    {
        public static void LogUser(IEventPublisher eventPublisher, UserId userId)
        {
            eventPublisher.Publish(new UserConnected(userId, DateTime.Now));
        }
    }
}
