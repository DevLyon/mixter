using System;
using System.Collections.Generic;
using Mixter.Domain.Identity;
using Mixter.Domain.Identity.Events;
using NFluent;
using Xunit;

namespace Mixter.Domain.Tests.Identity
{
    public class SessionHandlerTest
    {
        private readonly SessionHandler _handler;
        private readonly SessionsRepositoryFake _repository;
        
        public SessionHandlerTest()
        {
            _repository = new SessionsRepositoryFake();
            _handler = new SessionHandler(_repository);
        }

        [Fact]
        public void WhenUserConnectedThenStoreSessionProjection()
        {
            var userConnected = new UserConnected(SessionId.Generate(), new UserId("user@mixit.fr"), DateTime.Now);

            _handler.Handle(userConnected);

            Check.That(_repository.Projections)
                 .ContainsExactly(new SessionProjection(userConnected.SessionId, userConnected.UserId, SessionState.Enabled));
        }

        [Fact]
        public void WhenUserDiconnectedThenUpdateSessionProjectionAndEnableDisconnectedFlag()
        {
            var userConnected = new UserConnected(SessionId.Generate(), new UserId("user@mixit.fr"), DateTime.Now);
            _handler.Handle(userConnected);

            _handler.Handle(new UserDisconnected(userConnected.SessionId, userConnected.UserId));

            Check.That(_repository.Projections)
                 .ContainsExactly(new SessionProjection(userConnected.SessionId, userConnected.UserId, SessionState.Disabled));
        }

        private class SessionsRepositoryFake : ISessionsRepository
        {
            private readonly Dictionary<SessionId, SessionProjection> _projectionsById = new Dictionary<SessionId, SessionProjection>();

            public IEnumerable<SessionProjection> Projections
            {
                get { return _projectionsById.Values; }
            }

            public void Save(SessionProjection projection)
            {
                _projectionsById[projection.SessionId] = projection;
            }

            public void ReplaceBy(SessionProjection projection)
            {
                Save(projection);
            }

            public UserId? GetUserIdOfSession(SessionId sessionId)
            {
                throw new NotImplementedException();
            }

            public Session GetSession(SessionId sessionId)
            {
                throw new NotImplementedException();
            }
        }
    }
}
