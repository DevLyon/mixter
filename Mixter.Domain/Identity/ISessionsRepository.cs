namespace Mixter.Domain.Identity
{
    [Repository]
    public interface ISessionsRepository
    {
        void Save(SessionProjection projection);

        [Query]
        UserId? GetUserIdOfSession(SessionId sessionId);
        
        [Query]
        Session GetSession(SessionId sessionId);
    }
}