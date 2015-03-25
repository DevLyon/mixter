using Mixter.Infrastructure;

namespace Mixter.Tests.Infrastructure
{
    public class EventAHandler : IEventHandler<EventA>
    {
        public bool IsCalled { get; private set; }

        public void Handle(EventA evt)
        {
            IsCalled = true;
        }
    }
}