using Mixter.Domain;

namespace Mixter.Infrastructure
{
    public interface IEventSubscriber
    {
        void Subscribe(IEventHandler handler);
    }
}