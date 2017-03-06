package mixter.domain.identity.handlers

import mixter.domain.identity.event.{UserConnected, UserDisconnected}
import mixter.domain.identity.{SessionProjection, SessionProjectionRepository, SessionStatus}

class RegisterSession(repository:SessionProjectionRepository){
  def apply(event: UserConnected):Unit = {
    val projection = SessionProjection(event.sessionId,  event.id, SessionStatus.CONNECTED)
    repository.save(projection)
  }
  def apply(event: UserDisconnected):Unit = {
    val projection = SessionProjection(event.sessionId,  event.id, SessionStatus.DISCONNECTED)
    repository.replaceBy(projection)
  }
}
