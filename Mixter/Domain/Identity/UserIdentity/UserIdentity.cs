using Mixter.Domain.Core;
using Mixter.Domain.Identity.UserIdentity.Events;

namespace Mixter.Domain.Identity.UserIdentity
{
    public class UserIdentity
    {
        public static void Register(IEventPublisher eventPublisher, UserId userId)
        {
            eventPublisher.Publish(new UserRegistered(userId));
        }
    }
}