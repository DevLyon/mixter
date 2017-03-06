package mixter.domain.message

import java.util.UUID

import mixter.domain.AggregateId

case class MessageId(id:String) extends AggregateId {
  override val toString: String = id
}

object MessageId{
  def generate():MessageId = MessageId(UUID.randomUUID.toString)
}