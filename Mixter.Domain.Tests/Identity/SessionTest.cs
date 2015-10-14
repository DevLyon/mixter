using System;
using Mixter.Domain.Identity;
using Mixter.Domain.Identity.Events;
using NFluent;
using Xunit;

namespace Mixter.Domain.Tests.Identity
{
    public class SessionTest
    {
        private static readonly SessionId SessionId = SessionId.Generate();
        private static readonly UserId UserId = new UserId("user@mixit.fr");

        private readonly EventPublisherFake _eventPublisher = new EventPublisherFake();

        [Fact]
        public void WhenUserLogoutThenRaiseUserDisconnected()
        {
            var session = new Session(new UserConnected(SessionId, UserId, DateTime.Now));

            session.Logout(_eventPublisher);

            Check.That(_eventPublisher.Events)
                 .Contains(new UserDisconnected(SessionId, UserId));
        }
    }
}
