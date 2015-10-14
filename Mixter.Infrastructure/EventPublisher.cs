using System.Collections.Generic;
using System.Linq;
using Mixter.Domain;

namespace Mixter.Infrastructure
{
    public class EventPublisher : IEventPublisher
    {
        private readonly IList<IEventHandler> _handlers = new List<IEventHandler>();

        public void Subscribe(IEventHandler handler)
        {
            _handlers.Add(handler);
        }

        public void Publish<TEvent>(TEvent evt) where TEvent : IDomainEvent
        {
            foreach (var handler in _handlers.OfType<IEventHandler<TEvent>>())
            {
                handler.Handle(evt);
            }
        }
    }
}
