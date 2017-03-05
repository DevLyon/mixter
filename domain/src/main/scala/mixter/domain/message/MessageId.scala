package mixter.domain.message

import java.util.UUID

import mixter.domain.AggregateId

case class MessageId(id:String) extends AggregateId

object MessageId{
  def generate():MessageId = MessageId(UUID.randomUUID.toString)
}