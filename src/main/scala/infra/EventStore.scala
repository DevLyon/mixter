package infra

import domain.{AggregateId, Event}

trait EventStore {
  def eventsOfAggregate(aggregateId: AggregateId):Seq[Event]
}
