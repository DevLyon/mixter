using System;
using System.Collections.Generic;

namespace Mixter.Domain
{
    public abstract class DecisionProjectionBase
    {
        private readonly Dictionary<Type, Action<IDomainEvent>> _handlersByType = new Dictionary<Type, Action<IDomainEvent>>();

        public void Apply(IDomainEvent evt)
        {
            Action<IDomainEvent> apply;
            if (_handlersByType.TryGetValue(evt.GetType(), out apply))
            {
                apply(evt);
            }
        }

        protected void AddHandler<T>(Action<T> apply)
            where T : IDomainEvent
        {
            _handlersByType.Add(typeof(T), o => apply((T)o));
        }
    }
}
