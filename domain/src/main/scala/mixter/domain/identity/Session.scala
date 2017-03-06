package mixter.domain.identity

import mixter.domain.EventPublisher
import mixter.domain.identity.event._

object Session {
  private case class DecisionProjection(userId:UserId, sessionId: SessionId, active:Boolean){
    def apply(event:UserSessionEvent):DecisionProjection= event match{
      case UserDisconnected(_,_) => copy(active=false)
      case UserConnected(_,_,_) => this
    }
  }
  private object DecisionProjection{
    def of(userConnected: UserConnected)=DecisionProjection(userConnected.userId, userConnected.sessionId, active = true)
  }
}

case class Session(userConnected:UserConnected, history: Seq[UserSessionEvent]=Seq.empty){
  private val projection={
    val seed = Session.DecisionProjection.of(userConnected)
    history.foldLeft(seed)((acc, ev)=>acc(ev))
  }

  def logout()(implicit ep:EventPublisher):Unit =
    if(projection.active){
      ep.publish(UserDisconnected(projection.sessionId, projection.userId))
    }

}
