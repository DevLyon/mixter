package mixter.infra

import mixter.domain.Event

case class EventA(id: AnAggregateId) extends Event