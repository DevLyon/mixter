package domain.message.event

import domain.message.{MessageId, UserId}

case class MessageQuacked(messageId:MessageId, message:String, author:UserId)
