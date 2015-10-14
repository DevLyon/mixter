using System.Linq;
using Mixter.Domain.Identity;

namespace Mixter.Infrastructure
{
    public class UserIdentitiesRepository : IUserIdentitiesRepository
    {
        private readonly EventsStore _eventStore;

        public UserIdentitiesRepository(EventsStore eventStore)
        {
            _eventStore = eventStore;
        }

        public UserIdentity GetUserIdentity(UserId userId)
        {
            var events = _eventStore.GetEventsOfAggregate(userId).ToArray();
            if (!events.Any())
            {
                throw new UnknownUserIdentity(userId);
            }

            return new UserIdentity(events);
        }
    }
}
