namespace Mixter.Domain
{
    public interface IDomainEvent
    {
        object GetAggregateId();
    }
}