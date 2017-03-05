package mixter.infra

import mixter.domain.{AggregateId, Event}

trait EventStore {
  def store(event:Event):Unit
  def eventsOfAggregate(aggregateId: AggregateId):Seq[Event]
}
