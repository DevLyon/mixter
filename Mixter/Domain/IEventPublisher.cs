namespace Mixter.Domain
{
    public interface IEventPublisher
    {
        void Publish(IDomainEvent evt);
    }
}