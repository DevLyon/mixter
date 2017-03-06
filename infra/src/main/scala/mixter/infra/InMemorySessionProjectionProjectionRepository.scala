package mixter.infra

import mixter.domain.identity.{SessionId, SessionProjection, SessionProjectionRepository, SessionStatus}

class InMemorySessionProjectionProjectionRepository extends SessionProjectionRepository{
  var sessions = Map.empty[SessionId,SessionProjection]

  override def save(sessionProjection: SessionProjection): Unit =
    sessions+=sessionProjection.sessionId -> sessionProjection

  override def replaceBy(sessionProjection: SessionProjection): Unit =
    save(sessionProjection)

  override def getById(id: SessionId): Option[SessionProjection] =
    sessions.get(id).filterNot(_.status==SessionStatus.DISCONNECTED)
}
