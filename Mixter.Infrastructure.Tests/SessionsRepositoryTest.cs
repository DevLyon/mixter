using Mixter.Domain.Identity;
using NFluent;
using Xunit;

namespace Mixter.Infrastructure.Tests
{
    public class SessionsRepositoryTest
    {
        private static readonly SessionId SessionId = SessionId.Generate();
        private static readonly UserId UserId = new UserId("user1@mix-it.fr");

        private readonly SessionsRepository _repository = new SessionsRepository();

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
    }
}
