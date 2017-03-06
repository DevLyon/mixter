package mixter.domain.message

import mixter.domain.identity.UserId
import mixter.domain.message.Message.DecisionProjection
import mixter.domain.message.event.{MessageDeleted, MessageEvent, MessageQuacked, MessageRequacked}
import mixter.domain.{Aggregate, EventPublisher}

case class Message(messageQuacked: MessageQuacked, events:Traversable[MessageEvent]) extends Aggregate {


  override type Id = MessageId
  override type AggregateEvent = MessageEvent
  override type InitialEvent = MessageQuacked

  private val projection = events.foldLeft(DecisionProjection.of(messageQuacked))((acc,event)=> acc(event))

  def requack(requacker: UserId, authorId:UserId, message:String)(implicit ep:EventPublisher): Unit =
    if(!projection.publishers.contains(requacker)){
      ep.publish(MessageRequacked(projection.messageId,requacker, authorId, message))
    }

  def delete(userId: UserId)(implicit ep:EventPublisher):Unit =
    if(projection.authorId==userId){
      ep.publish(MessageDeleted(projection.messageId))
    }
}

object Message {
  def quack(message: String, author: UserId)
           (implicit idGen:()=>MessageId=MessageId.generate, ep:EventPublisher)
  : Unit = {
    ep.publish(MessageQuacked(idGen(),message, author))
  }
  case class DecisionProjection(messageId:MessageId, authorId:UserId, publishers:Set[UserId]){
    def apply(messageEvent:MessageEvent):DecisionProjection = messageEvent match {
      case MessageRequacked(_,requacker, _, _)=> copy(publishers=publishers+requacker)
      case MessageDeleted(_) => this // effectless for now
      case MessageQuacked(_,_,_) => this // invalid, a message can only be requacked once
    }
  }
  object DecisionProjection{
    def of(messageQuacked: MessageQuacked) = DecisionProjection(messageQuacked.id, messageQuacked.author, Set(messageQuacked.author))
  }
}
