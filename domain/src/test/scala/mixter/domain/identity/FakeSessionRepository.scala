package mixter.domain.identity

import org.scalatest.{Suite, SuiteMixin}


trait WithSessionRepository extends SuiteMixin { this: Suite =>
  def withSessionRepository(test: FakeSessionRepository=>Any): Any = {
    val sessionRepository = new FakeSessionRepository()
    test(sessionRepository)
  }


}

private[identity] class FakeSessionRepository() extends SessionRepository {
  var sessions = Map.empty[SessionId,SessionProjection]

  def getSessions:Set[SessionProjection] =
    sessions.values.toSet

  override def save(sessionProjection: SessionProjection): Unit =
    sessions+=sessionProjection.sessionId -> sessionProjection

  override def replaceBy(sessionProjection: SessionProjection): Unit =
    save(sessionProjection)

  override def getById(id: SessionId): Option[SessionProjection] = sessions.get(id)
}
