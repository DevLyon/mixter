using Mixter.Domain.Identity;
using NFluent;
using Xunit;

namespace Mixter.Infrastructure.Tests
{
    public class SessionsRepositoryTest
    {
        private readonly SessionsRepository _repository = new SessionsRepository();

        [Fact]
        public void GivenNoProjectionsWhenGetUserIdOfSessionThenReturnEmpty()
        {
            var userId = _repository.GetUserIdOfSession(SessionId.Generate());

            Check.That(userId.HasValue).IsFalse();
        }

        [Fact]
        public void GivenSeveralUserConnectedWhenGetUserIdOfSessionThenReturnUserIdOfThisSession()
        {
            var sessionId = SessionId.Generate();
            var userId = new UserId("user1@mix-it.fr");

            _repository.Save(new SessionProjection(sessionId, userId, SessionState.Enabled));
            _repository.Save(new SessionProjection(SessionId.Generate(), new UserId("user2@mix-it.fr"), SessionState.Enabled));

            Check.That(_repository.GetUserIdOfSession(sessionId)).IsEqualTo(userId);
        }
    }
}
