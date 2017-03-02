package domain.identity

import java.util.UUID

import domain.AggregateId

case class SessionId(value:String=UUID.randomUUID().toString) extends AggregateId{
  require(value != null, "A session id cannot be null")
  require(value.nonEmpty, "A session id cannot be empty")

  override val toString: String = value
}