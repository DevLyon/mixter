namespace Mixter.Domain
{
    public interface IEventPublisher
    {
        void Publish<TEvent>(TEvent evt) where TEvent : IDomainEvent;
    }
}