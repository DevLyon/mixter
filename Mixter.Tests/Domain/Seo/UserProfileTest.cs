using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mixter.Domain.Core;
using Mixter.Domain.Identity;
using Mixter.Domain.Seo.UserProfiles;
using Mixter.Domain.Seo.UserProfiles.Events;
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
        private static readonly UserProfileId UserProfileId = new UserProfileId(UserId);

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
                 .Contains(new UserProfileCreated(UserProfileId, UserId, FirstName, LastName));
        }

        [TestMethod]
        public void WhenUpdateDescriptionThenRaiseUserProfileUpdated()
        {
            var userProfile = new UserProfile(new UserProfileCreated(UserProfileId, UserId, FirstName, LastName));

            userProfile.UpdateDescription(_eventPublisher, "EventSourcing", "CQRS");

            Check.That(_eventPublisher.Events)
                 .Contains(new UserDescriptionUpdated(UserProfileId, "EventSourcing", "CQRS"));
        }

        [TestMethod]
        public void WhenUpdateDescriptionWithSameDescriptionThenNothing()
        {
            var userProfile = new UserProfile(new UserProfileCreated(UserProfileId, UserId, FirstName, LastName));

            userProfile.UpdateDescription(_eventPublisher, FirstName, LastName);

            Check.That(_eventPublisher.Events).IsEmpty();
        }
    }
}
