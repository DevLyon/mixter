using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mixter.Domain.Core;
using Mixter.Domain.Identity.UserIdentity;
using Mixter.Domain.Identity.UserIdentity.Events;
using Mixter.Tests.Infrastructure;
using NFluent;

namespace Mixter.Tests.Domain.Identity
{
    [TestClass]
    public class UserIdentityTest
    {
        [TestMethod]
        public void WhenRegisterThenRaiseUserRegisteredEvent()
        {
            var eventPublisher = new EventPublisherFake();

            UserIdentity.Register(eventPublisher, new UserId("user@mixit.fr"));

            Check.That(eventPublisher.Events).Contains(new UserRegistered(new UserId("user@mixit.fr")));
        }
    }
}
