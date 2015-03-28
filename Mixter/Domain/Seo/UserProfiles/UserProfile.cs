using Mixter.Domain.Core;

namespace Mixter.Domain.Seo.UserProfiles
{
    public class UserProfile
    {
        public static void Create(IEventPublisher eventPublisher, UserId userId, string firstName, string lastName)
        {
            var id = new UserProfileId(userId);
            eventPublisher.Publish(new UserProfileCreated(id, userId, firstName, lastName));
        }
    }
}