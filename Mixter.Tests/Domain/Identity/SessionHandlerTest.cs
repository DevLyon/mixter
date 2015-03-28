using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mixter.Domain.Identity;
using Mixter.Domain.Identity.Events;
using NFluent;

namespace Mixter.Tests.Domain.Identity
{
    [TestClass]
    public class SessionHandlerTest
    {
        private SessionHandler _handler;
        private SessionsRepositoryFake _repository;

        [TestInitialize]
        public void Initialize()
        {
            _repository = new SessionsRepositoryFake();
            _handler = new SessionHandler(_repository);
        }

        [TestMethod]
        public void WhenUserConnectedThenStoreSessionProjection()
        {
            var userConnected = new UserConnected(SessionId.Generate(), new UserId("user@mixit.fr"), DateTime.Now);

            _handler.Handle(userConnected);

            Check.That(_repository.Projections)
                 .ContainsExactly(new SessionProjection(userConnected.SessionId, userConnected.UserId));
        }

        private class SessionsRepositoryFake : ISessionsRepository
        {
            public SessionsRepositoryFake()
            {
                Projections = new List<SessionProjection>();
            }

            public IList<SessionProjection> Projections { get; private set; }

            public void Save(SessionProjection projection)
            {
                Projections.Add(projection);
            }
        }
    }
}
