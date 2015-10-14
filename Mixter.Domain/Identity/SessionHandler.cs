using Mixter.Domain.Identity.Events;

namespace Mixter.Domain.Identity
{
    [Handler]
    public class SessionHandler :
        IEventHandler<UserConnected>,
        IEventHandler<UserDisconnected>
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

        public void Handle(UserDisconnected evt)
        {
            _repository.Save(new SessionProjection(evt));
        }
    }
}
