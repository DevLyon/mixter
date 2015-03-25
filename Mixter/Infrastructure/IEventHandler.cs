using Mixter.Domain;

namespace Mixter.Infrastructure
{
    public interface IEventHandler
    {
    }

    public interface IEventHandler<TEvent> : IEventHandler
        where TEvent : IDomainEvent
    {
        void Handle(TEvent evt);
    }
}