namespace Mixter.Domain.Identity
{
    public interface ISessionsRepository
    {
        void Save(SessionProjection projection);

        UserId? GetUserIdOfSession(SessionId sessionId);
    }
}