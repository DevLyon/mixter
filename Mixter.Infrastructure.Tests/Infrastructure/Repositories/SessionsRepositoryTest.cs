using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mixter.Domain.Identity;
using Mixter.Domain.Identity.Events;
using Mixter.Infrastructure.Repositories;
using NFluent;

namespace Mixter.Infrastructure.Tests.Infrastructure.Repositories
{
    [TestClass]
    public class SessionsRepositoryTest
    {
        private static readonly SessionId SessionId = SessionId.Generate();
        private static readonly UserId UserId = new UserId("user1@mixit.fr");

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
            _repository.Save(new SessionProjection(new UserConnected(SessionId, UserId, DateTime.Now)));
            _repository.Save(new SessionProjection(new UserConnected(SessionId.Generate(), new UserId("user2@mixit.fr"), DateTime.Now)));

            var userIdOfSession = _repository.GetUserIdOfSession(SessionId);

            Check.That(userIdOfSession).IsEqualTo(UserId);
        }

        [TestMethod]
        public void GivenUserDisconnectedWhenGetUserIdOfThisSessionThenReturnEmpty()
        {
            _repository.Save(new SessionProjection(new UserDisconnected(SessionId, UserId)));

            var userIdOfSession = _repository.GetUserIdOfSession(SessionId);

            Check.That(userIdOfSession).IsNull();
        }

        [TestMethod]
        public void WhenReplaceProjectionThenUpdateProjection()
        {
            _repository.Save(new SessionProjection(new UserConnected(SessionId, UserId, DateTime.Now)));
            _repository.ReplaceBy(new SessionProjection(new UserDisconnected(SessionId, UserId)));

            var userIdOfSession = _repository.GetUserIdOfSession(SessionId);

            Check.That(userIdOfSession).IsNull();
        }
    }
}
