using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mixter.Domain.Identity;
using Mixter.Domain.Identity.Events;
using Mixter.Infrastructure.Tests.Infrastructure;
using NFluent;

namespace Mixter.Domain.Tests.Identity
{
    [TestClass]
    public class SessionTest
    {
        private static readonly SessionId SessionId = SessionId.Generate();
        private static readonly UserId UserId = new UserId("user@mixit.fr");

        private EventPublisherFake _eventPublisher;

        [TestInitialize]
        public void Initialize()
        {
            _eventPublisher = new EventPublisherFake();
        }

        [TestMethod]
        public void WhenUserLogoutThenRaiseUserDisconnected()
        {
            var session = new Session(new UserConnected(SessionId, UserId, DateTime.Now));

            session.Logout(_eventPublisher);

            Check.That(_eventPublisher.Events)
                 .Contains(new UserDisconnected(SessionId, UserId));
        }

        [TestMethod]
        public void GivenUserDisconnectedWhenUserLogoutThenNothing()
        {
            var session = new Session(
                new UserConnected(SessionId, UserId, DateTime.Now),
                new UserDisconnected(SessionId, UserId));

            session.Logout(_eventPublisher);

            Check.That(_eventPublisher.Events).IsEmpty();
        }
    }
}
