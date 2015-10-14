using System.Collections.Generic;
using System.Linq;
using Mixter.Domain;
using Mixter.Domain.Identity;

namespace Mixter.Infrastructure
{
    public class SessionsRepository : ISessionsRepository
    {
        private readonly EventsStore _eventsStore;
        private readonly IDictionary<SessionId, SessionProjection> _projectionsById = new Dictionary<SessionId, SessionProjection>();

        public SessionsRepository(EventsStore eventsStore)
        {
            _eventsStore = eventsStore;
        }

        public void Save(SessionProjection projection)
        {
            if (_projectionsById.ContainsKey(projection.SessionId))
            {
                _projectionsById[projection.SessionId] = projection;
                return;
            }

            _projectionsById.Add(projection.SessionId, projection);
        }

        public UserId? GetUserIdOfSession(SessionId sessionId)
        {
            if (!_projectionsById.ContainsKey(sessionId))
            {
                return null;
            }

            var projectionOfSession = _projectionsById[sessionId];
            return projectionOfSession.SessionState == SessionState.Enabled
                ? projectionOfSession.UserId
                : (UserId?)null;
        }

        public Session GetSession(SessionId sessionId)
        {
            var events = _eventsStore.GetEventsOfAggregate(sessionId).ToArray();
            if (!events.Any())
            {
                throw new UnknownSession(sessionId);
            }

            return new Session(events);
        }
    }
}
