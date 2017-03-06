package mixter.infra

import mixter.domain.Event

case class EventB(id: AnAggregateId) extends Event