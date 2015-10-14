using System;
using Mixter.Domain;
using Mixter.Domain.Identity;
using Mixter.Domain.Identity.Events;
using NFluent;
using Xunit;

namespace Mixter.Infrastructure.Tests
{
    public class SessionsRepositoryTest
    {
        private static readonly SessionId SessionId = SessionId.Generate();
        private static readonly UserId UserId = new UserId("user1@mix-it.fr");

        private readonly SessionsRepository _repository;
        private readonly EventsStore _eventsStore;

        public SessionsRepositoryTest()
        {
            _eventsStore = new EventsStore();
            _repository = new SessionsRepository(_eventsStore);
        }

        [Fact]
        public void GivenNoProjectionsWhenGetUserIdOfSessionThenReturnNone()
        {
            var userId = _repository.GetUserIdOfSession(SessionId.Generate());

            Check.That(userId.HasValue).IsFalse();
        }

        [Fact]
        public void GivenSeveralUserConnectedWhenGetUserIdOfSessionThenReturnUserIdOfThisSession()
        {
            _repository.Save(new SessionProjection(SessionId, UserId, SessionState.Enabled));
            _repository.Save(new SessionProjection(SessionId.Generate(), new UserId("user2@mix-it.fr"), SessionState.Enabled));

            Check.That(_repository.GetUserIdOfSession(SessionId)).IsEqualTo(UserId);
        }

        [Fact]
        public void GivenUserDisconnectedWhenGetUserIdOfSessionThenReturnNone()
        {
            _repository.Save(new SessionProjection(SessionId, UserId, SessionState.Disabled));

            Check.That(_repository.GetUserIdOfSession(SessionId).HasValue).IsFalse();
        }

        [Fact]
        public void GivenAlreadyProjectionWhenSaveSameProjectionThenUpdateProjection()
        {
            _repository.Save(new SessionProjection(SessionId, UserId, SessionState.Enabled));

            _repository.Save(new SessionProjection(SessionId, UserId, SessionState.Disabled));

            Check.That(_repository.GetUserIdOfSession(SessionId).HasValue).IsFalse();
        }

        [Fact]
        public void GivenUserConnectedWhenGetSessionThenReturnSessionAggregate()
        {
            _eventsStore.Store(new UserConnected(SessionId, UserId, DateTime.Now));

            var session = _repository.GetSession(SessionId);

            Check.That(session).IsNotNull();
        }

        [Fact]
        public void GivenNoEventsWhenGetSessionThenThrowUnknownSession()
        {
            Check.ThatCode(() => _repository.GetSession(SessionId)).Throws<UnknownSession>();
        }
    }
}
