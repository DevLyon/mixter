namespace Mixter.Domain.Identity
{
    public interface IUserIdentitiesRepository
    {
        UserIdentity GetUserIdentity(UserId userId);
    }
}