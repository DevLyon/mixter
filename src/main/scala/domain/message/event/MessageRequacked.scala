package domain.message.event

import domain.Event
import domain.message.{MessageId, UserId}

case class MessageRequacked(messageId:MessageId,requacker: UserId, authorId:UserId, content:String) extends Event
