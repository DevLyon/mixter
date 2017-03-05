package mixter.domain.message.event

import mixter.domain.Event
import mixter.domain.identity.UserId
import mixter.domain.message.MessageId

sealed trait MessageEvent extends Event
case class MessageQuacked(id:MessageId, message:String, author:UserId) extends MessageEvent
case class MessageRequacked(id:MessageId,requacker: UserId, author: UserId, message:String) extends MessageEvent
