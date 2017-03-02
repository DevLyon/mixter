package infra

import domain.{AggregateId, Event}

trait EventStore {
  def store(event:Event):Unit
  def eventsOfAggregate(aggregateId: AggregateId):Seq[Event]
}
