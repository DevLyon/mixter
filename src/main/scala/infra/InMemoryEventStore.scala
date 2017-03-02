package infra
import domain.{AggregateId, Event}

private[infra] class InMemoryEventStore extends EventStore {
  var events = Seq.empty[Event]
  override def eventsOfAggregate(aggregateId: AggregateId): Seq[Event] =
    events
}
