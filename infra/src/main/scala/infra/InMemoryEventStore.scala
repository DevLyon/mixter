package infra
import domain.{AggregateId, Event}

private[infra] class InMemoryEventStore extends EventStore {
  private var events = Map.empty[AggregateId, Seq[Event]].withDefaultValue(Seq.empty[Event])

  override def store(event: Event): Unit =
    events = events + (event.id -> (events(event.id) :+ event))

  override def eventsOfAggregate(id: AggregateId): Seq[Event] =
    events(id)

}
