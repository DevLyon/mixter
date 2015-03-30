using System;
using System.Collections.Generic;
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

        public EventPublisher(Func<IEventPublisher, IEnumerable<IEventHandler>> generateHandlers)
        {
            _handlers = generateHandlers(this).ToArray();
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