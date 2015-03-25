using System.Linq;
using Mixter.Domain;

namespace Mixter.Infrastructure
{
    public class EventPublisher : IEventPublisher
    {
        private readonly IEventHandler[] _handlers;

        public EventPublisher(params IEventHandler[] handlers)
        {
            _handlers = handlers;
        }

        public void Publish<TEvent>(TEvent evt) where TEvent : IDomainEvent
        {
            ((IEventHandler<TEvent>)_handlers.First()).Handle(evt);
        }
    }
}