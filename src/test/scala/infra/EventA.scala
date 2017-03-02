package infra

import domain.Event

case class EventA(anAggregateId: AnAggregateId) extends Event