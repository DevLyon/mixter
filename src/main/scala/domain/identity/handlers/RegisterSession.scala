package domain.identity.handlers

import domain.identity.event.{UserConnected, UserDisconnected}
import domain.identity.{SessionProjection, SessionRepository, SessionStatus}

class RegisterSession(sessionRepository:SessionRepository){
  def apply(userConnected:UserConnected):Unit = {
    val projection = SessionProjection(userConnected.sessionId,  userConnected.userId, SessionStatus.CONNECTED)
    sessionRepository.save(projection)
  }
  def apply(userDisconnected: UserDisconnected):Unit = {
    val projection = SessionProjection(userDisconnected.sessionId,  userDisconnected.userId, SessionStatus.DISCONNECTED)
    sessionRepository.replaceBy(projection)
  }
}
