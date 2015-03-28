using Mixter.Domain.Identity.Events;
using Mixter.Infrastructure;

namespace Mixter.Domain.Identity
{
    public class SessionHandler : IEventHandler<UserConnected>
    {
        private readonly ISessionsRepository _repository;

        public SessionHandler(ISessionsRepository repository)
        {
            _repository = repository;
        }

        public void Handle(UserConnected evt)
        {
            _repository.Save(new SessionProjection(evt));
        }
    }
}