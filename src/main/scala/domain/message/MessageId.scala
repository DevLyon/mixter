package domain.message

import java.util.UUID

case class MessageId(id:String)

object MessageId{
  def generate():MessageId = MessageId(UUID.randomUUID.toString)
}