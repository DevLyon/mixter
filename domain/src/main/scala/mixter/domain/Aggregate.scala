package mixter.domain

trait Aggregate {
  type Id<:AggregateId
  type AggregateEvent<:Event
  type InitialEvent<:AggregateEvent
}
