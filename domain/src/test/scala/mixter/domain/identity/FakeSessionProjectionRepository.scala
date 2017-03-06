package mixter.domain.identity

import org.scalatest.{Suite, SuiteMixin}


trait WithSessionRepository extends SuiteMixin { this: Suite =>
  def withSessionRepository(test: FakeSessionProjectionRepository=>Any): Any = {
    val sessionRepository = new FakeSessionProjectionRepository()
    test(sessionRepository)
  }


}

private[identity] class FakeSessionProjectionRepository() extends SessionProjectionRepository {
  var sessions = Map.empty[SessionId,SessionProjection]

  def getSessions:Set[SessionProjection] =
    sessions.values.toSet

  override def save(sessionProjection: SessionProjection): Unit =
    sessions+=sessionProjection.sessionId -> sessionProjection

  override def replaceBy(sessionProjection: SessionProjection): Unit =
    save(sessionProjection)

  override def getById(id: SessionId): Option[SessionProjection] = sessions.get(id)
}
