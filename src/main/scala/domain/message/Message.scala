package domain.message

import domain.EventPublisher
import domain.message.event.MessageQuacked

object Message {
  def quack(message: String, author: UserId)
           (implicit idGen:()=>MessageId=MessageId.generate, ep:EventPublisher)
  : Unit = {
    ep.publish(MessageQuacked(idGen(),message, author))
  }
}
