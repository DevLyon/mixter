package mixter.infra.repositories

import mixter.domain.Aggregate
import mixter.infra.EventStore

abstract class AggregateRepository[A<:Aggregate]{
  protected def store:EventStore
  def getById(id: A#Id):Option[A] ={
    val history = store.eventsOfAggregate(id)
    history.headOption.map( initialEvent=>
      build(initialEvent.asInstanceOf[A#InitialEvent], history.tail.map(_.asInstanceOf[A#AggregateEvent]))
    )
  }
  protected def build(initialEvent:A#InitialEvent, history:Seq[A#AggregateEvent]):A
}
