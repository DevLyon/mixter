package domain.identity

import domain.EventPublisher
import domain.identity.event.{UserConnected, UserDisconnected}

object Session {
  private case class DecisionProjection(userConnected: UserConnected){
    val userId: UserId = userConnected.userId
    val sessionId: SessionId = userConnected.sessionId
  }
}

case class Session(userConnected:UserConnected){
  private val projection=Session.DecisionProjection(userConnected)

  def logout()(implicit ep:EventPublisher):Unit =
    ep.publish(UserDisconnected(projection.sessionId, projection.userId))
}
