package mixter.domain.identity.handlers

import mixter.domain.identity.event.{UserConnected, UserDisconnected}
import mixter.domain.identity.{SessionProjection, SessionRepository, SessionStatus}

class RegisterSession(sessionRepository:SessionRepository){
  def apply(event: UserConnected):Unit = {
    val projection = SessionProjection(event.sessionId,  event.id, SessionStatus.CONNECTED)
    sessionRepository.save(projection)
  }
  def apply(event: UserDisconnected):Unit = {
    val projection = SessionProjection(event.sessionId,  event.id, SessionStatus.DISCONNECTED)
    sessionRepository.replaceBy(projection)
  }
}
