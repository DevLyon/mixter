namespace Mixter.Domain.Identity
{
    [Repository]
    public interface IUserIdentitiesRepository
    {
        [Query]
        UserIdentity GetUserIdentity(UserId userId);
    }
}