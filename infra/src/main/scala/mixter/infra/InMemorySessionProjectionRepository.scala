package mixter.infra

import mixter.domain.identity.{SessionId, SessionProjection, SessionRepository}

class InMemorySessionProjectionRepository extends SessionRepository{
  override def save(sessionProjection: SessionProjection): Unit = ???

  override def replaceBy(sessionProjection: SessionProjection): Unit = ???

  override def getById(id: SessionId): Option[SessionProjection] = None
}
