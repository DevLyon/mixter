package mixter.domain.message

import mixter.domain.EventPublisher
import mixter.domain.identity.UserId
import mixter.domain.message.event.{MessageEvent, MessageQuacked, MessageRequacked}
import mixter.domain.message.Message.DecisionProjection

case class Message(messageQuacked: MessageQuacked, events:Traversable[MessageEvent]) {
  private val projection = events.foldLeft(DecisionProjection.of(messageQuacked))((acc,event)=> acc(event))

  def requack(requacker: UserId, authorId:UserId, message:String)(implicit ep:EventPublisher): Unit =
    if(!projection.publishers.contains(requacker)){
      ep.publish(MessageRequacked(projection.messageId,requacker, authorId, message))
    }
}

object Message {
  def quack(message: String, author: UserId)
           (implicit idGen:()=>MessageId=MessageId.generate, ep:EventPublisher)
  : Unit = {
    ep.publish(MessageQuacked(idGen(),message, author))
  }
  case class DecisionProjection(messageId:MessageId, publishers:Set[UserId]){
    def apply(messageEvent:MessageEvent):DecisionProjection = messageEvent match {
      case MessageRequacked(_,requacker, _, _)=> copy(publishers=publishers+requacker)
      case MessageQuacked(_,_,_) => this// invalid, a message can only be requacked once
    }
  }
  object DecisionProjection{
    def of(messageQuacked: MessageQuacked) = DecisionProjection(messageQuacked.id, Set(messageQuacked.author))
  }
}
