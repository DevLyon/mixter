using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mixter.Domain.Identity;
using Mixter.Domain.Identity.Events;
using Mixter.Tests.Infrastructure;
using NFluent;

namespace Mixter.Tests.Domain.Identity
{
    [TestClass]
    public class SessionTest
    {
        [TestMethod]
        public void WhenUserLogoutThenRaiseUserDisconnected()
        {
            var eventPublisher = new EventPublisherFake();
            var sessionId = SessionId.Generate();
            var userId = new UserId("user@mixit.fr");
            var session = new Session(new UserConnected(sessionId, userId, DateTime.Now));

            session.Logout(eventPublisher);

            Check.That(eventPublisher.Events)
                 .Contains(new UserDisconnected(sessionId, userId));
        }
    }
}
