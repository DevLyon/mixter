using Mixter.Domain.Identity;
using Mixter.Domain.Identity.Events;
using NFluent;
using Xunit;

namespace Mixter.Domain.Tests.Identity
{
    public class UserIdentityTest
    {
        private static readonly UserId UserId = new UserId("user@mixit.fr");

        private readonly EventPublisherFake _eventPublisher;
        
        public UserIdentityTest()
        {
            _eventPublisher = new EventPublisherFake();
        }

        [Fact]
        public void WhenRegisterThenRaiseUserRegisteredEvent()
        {
            UserIdentity.Register(_eventPublisher, UserId);

            Check.That(_eventPublisher.Events).Contains(new UserRegistered(UserId));
        }
    }
}
