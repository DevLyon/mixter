using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mixter.Domain.Core;
using Mixter.Domain.Seo.UserProfiles;
using Mixter.Tests.Infrastructure;
using NFluent;

namespace Mixter.Tests.Domain.Seo
{
    [TestClass]
    public class UserProfileTest
    {
        private const string FirstName = "Joe";
        private const string LastName = "Indien";

        private static readonly UserId UserId = new UserId("joe.indien@mixit.fr");

        private EventPublisherFake _eventPublisher;

        [TestInitialize]
        public void Initialize()
        {
            _eventPublisher = new EventPublisherFake();
        }

        [TestMethod]
        public void WhenCreateUserProfileThenRaiseUserProfileCreated()
        {
            UserProfile.Create(_eventPublisher, UserId, FirstName, LastName);

            Check.That(_eventPublisher.Events)
                 .Contains(new UserProfileCreated(new UserProfileId(UserId), UserId, FirstName, LastName));
        }
    }
}
