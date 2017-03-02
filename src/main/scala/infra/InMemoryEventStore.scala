package infra
import domain.{AggregateId, Event}

private[infra] class InMemoryEventStore extends EventStore {
  var events = Seq.empty[Event]

  override def store(event: Event): Unit =
    events = events :+ event

  override def eventsOfAggregate(aggregateId: AggregateId): Seq[Event] =
    events

}
