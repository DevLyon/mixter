package domain.identity

import java.time.{LocalDateTime, ZoneOffset}

import domain.EventPublisher
import domain.identity.event.{UserConnected, UserRegistered}

case class UserIdentity(userRegistered: UserRegistered) {
  import UserIdentity._

  private val projection=DecisionProjection.of(userRegistered)

  def logIn()(implicit ep:EventPublisher):Unit= {
    val sessionId = SessionId()
    ep.publish(UserConnected(sessionId, projection.userId, LocalDateTime.now(ZoneOffset.UTC)))
  }
}

object UserIdentity {
  def register(userId: UserId)(implicit ep:EventPublisher)=
    ep.publish(UserRegistered(userId))

  private case class DecisionProjection(userId: UserId)

  private object DecisionProjection{
    def of(userRegistered: UserRegistered)=DecisionProjection(userRegistered.userId)
  }
}
