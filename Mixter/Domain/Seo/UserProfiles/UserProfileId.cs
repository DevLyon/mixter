using Mixter.Domain.Core;

namespace Mixter.Domain.Seo.UserProfiles
{
    public struct UserProfileId
    {
        public UserId UserId { get; private set; }

        public UserProfileId(UserId userId) : this()
        {
            UserId = userId;
        }
    }
}