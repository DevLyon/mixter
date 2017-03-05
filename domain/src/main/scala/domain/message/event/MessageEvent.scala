package domain.message.event

import domain.Event
import domain.identity.UserId
import domain.message.MessageId

sealed trait MessageEvent extends Event
case class MessageQuacked(id:MessageId, message:String, author:UserId) extends MessageEvent
case class MessageRequacked(id:MessageId,requacker: UserId, author: UserId, message:String) extends MessageEvent
