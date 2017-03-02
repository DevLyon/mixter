package domain.message

import domain.EventPublisher
import domain.message.event.{MessageQuacked, MessageRequacked}

case class Message(quackEvent:MessageQuacked) {
  def requack(requacker: UserId, authorId:UserId, message:String)(implicit ep:EventPublisher): Unit =
    ep.publish(MessageRequacked(quackEvent.messageId,requacker, authorId, message))
}

object Message {
  def quack(message: String, author: UserId)
           (implicit idGen:()=>MessageId=MessageId.generate, ep:EventPublisher)
  : Unit = {
    ep.publish(MessageQuacked(idGen(),message, author))
  }
}
