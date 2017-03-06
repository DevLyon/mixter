package mixter.domain.identity

import java.time.{LocalDateTime, ZoneOffset}

import mixter.domain.identity.event.{UserConnected, UserEvent, UserRegistered}
import mixter.domain.{Aggregate, EventPublisher}

case class UserIdentity(userRegistered: UserRegistered, history:Seq[UserEvent]=Seq.empty) extends Aggregate {
  import UserIdentity._

  override type Id = UserId
  override type AggregateEvent = UserEvent
  override type InitialEvent = UserRegistered

  private val projection={
    val seed = DecisionProjection.of(userRegistered)
    history.foldLeft(seed)((acc, ev)=>acc(ev))
  }

  def logIn()(implicit ep:EventPublisher):Unit= {
    val sessionId = SessionId()
    ep.publish(UserConnected(sessionId, projection.userId, LocalDateTime.now(ZoneOffset.UTC)))
  }
}

object UserIdentity {
  def register(userId: UserId)(implicit ep:EventPublisher)=
    ep.publish(UserRegistered(userId))

  private case class DecisionProjection(userId: UserId){
    def apply(userEvent: UserEvent)=this
  }

  private object DecisionProjection{
    def of(userRegistered: UserRegistered)=DecisionProjection(userRegistered.id)
  }
}
