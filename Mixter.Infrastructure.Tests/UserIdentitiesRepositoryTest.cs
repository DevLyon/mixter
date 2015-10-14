using Mixter.Domain.Identity;
using Mixter.Domain.Identity.Events;
using NFluent;
using Xunit;

namespace Mixter.Infrastructure.Tests
{
    public class UserIdentitiesRepositoryTest
    {
        private readonly UserIdentitiesRepository _repository;
        private readonly EventsStore _eventStore;
        private static readonly UserId UserId = new UserId("user@mix-it.fr");

        public UserIdentitiesRepositoryTest()
        {
            _eventStore = new EventsStore();
            _repository = new UserIdentitiesRepository(_eventStore);
        }

        [Fact]
        public void GivenUserRegisteredWhenGetUserIdentityThenReturnUserIdentityAggregate()
        {
            _eventStore.Store(new UserRegistered(UserId));

            var user = _repository.GetUserIdentity(UserId);

            Check.That(user).IsNotNull();
        }

        [Fact]
        public void GivenNoEventsWhenGetUserIdentityThenThrowUnknownUserIdentity()
        {
            Check.ThatCode(() => _repository.GetUserIdentity(UserId)).Throws<UnknownUserIdentity>();
        }
    }
}
