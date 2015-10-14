using Mixter.Domain.Identity.Events;

namespace Mixter.Domain.Identity
{
    [Aggregate]
    public class UserIdentity
    {
        [Command]
        public static void Register(IEventPublisher eventPublisher, UserId userId)
        {
            eventPublisher.Publish(new UserRegistered(userId));
        }
    }
}
