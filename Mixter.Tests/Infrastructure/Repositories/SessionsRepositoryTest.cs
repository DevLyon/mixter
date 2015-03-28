using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mixter.Domain.Identity;
using Mixter.Domain.Identity.Events;
using Mixter.Infrastructure.Repositories;
using NFluent;

namespace Mixter.Tests.Infrastructure.Repositories
{
    [TestClass]
    public class SessionsRepositoryTest
    {
        private SessionsRepository _repository;

        [TestInitialize]
        public void Initialize()
        {
            _repository = new SessionsRepository();
        }

        [TestMethod]
        public void GivenNoProjectionsWhenGetUserIdThenReturnEmpty()
        {
            Check.That(_repository.GetUserIdOfSession(SessionId.Generate())).IsNull();
        }

        [TestMethod]
        public void GivenSeveralUserConnectedWhenGetUserIdOfASessionThenReturnUserIdOfThisSession()
        {
            var sessionId1 = SessionId.Generate();
            var userId1 = new UserId("user1@mixit.fr");
            _repository.Save(new SessionProjection(new UserConnected(sessionId1, userId1, DateTime.Now)));
            _repository.Save(new SessionProjection(new UserConnected(SessionId.Generate(), new UserId("user2@mixit.fr"), DateTime.Now)));

            var userIdOfSession = _repository.GetUserIdOfSession(sessionId1);

            Check.That(userIdOfSession).IsEqualTo(userId1);
        }

        [TestMethod]
        public void GivenUserDisconnectedWhenGetUserIdOfThisSessionThenReturnEmpty()
        {
            var sessionId1 = SessionId.Generate();
            var userId1 = new UserId("user1@mixit.fr");
            _repository.Save(new SessionProjection(new UserDisconnected(sessionId1, userId1)));

            var userIdOfSession = _repository.GetUserIdOfSession(sessionId1);

            Check.That(userIdOfSession).IsNull();
        }

        [TestMethod]
        public void WhenReplaceProjectionThenUpdateProjection()
        {
            var sessionId1 = SessionId.Generate();
            var userId1 = new UserId("user1@mixit.fr");
            _repository.Save(new SessionProjection(new UserConnected(sessionId1, userId1, DateTime.Now)));
            _repository.ReplaceBy(new SessionProjection(new UserDisconnected(sessionId1, userId1)));

            var userIdOfSession = _repository.GetUserIdOfSession(sessionId1);

            Check.That(userIdOfSession).IsNull();
        }
    }
}
