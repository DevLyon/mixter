using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mixter.Domain.Identity;
using Mixter.Domain.Identity.Events;
using Mixter.Infrastructure.Tests;
using NFluent;

namespace Mixter.Domain.Tests.Identity
{
    [TestClass]
    public class UserIdentityTest
    {
        private static readonly UserId UserId = new UserId("user@mixit.fr");

        private EventPublisherFake _eventPublisher;

        [TestInitialize]
        public void Initialize()
        {
            _eventPublisher = new EventPublisherFake();
        }

        [TestMethod]
        public void WhenRegisterThenRaiseUserRegisteredEvent()
        {
            UserIdentity.Register(_eventPublisher, UserId);

            Check.That(_eventPublisher.Events).Contains(new UserRegistered(UserId));
        }

        [TestMethod]
        public void GivenUserRegisteredWhenLogThenRaiseUserConnectedEvent()
        {
            var userIdentity = new UserIdentity(new UserRegistered(UserId));

            userIdentity.Log(_eventPublisher);

            var evt = _eventPublisher.Events.OfType<UserConnected>().First();
            Check.That(evt.UserId).IsEqualTo(UserId);
            Check.That(evt.ConnectedAt).IsBeforeOrEqualTo(DateTime.Now).And.IsAfterOrEqualTo(DateTime.Now.AddSeconds(-1));
        }
    }
}
