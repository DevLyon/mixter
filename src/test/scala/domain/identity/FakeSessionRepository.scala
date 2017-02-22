package domain.identity

import org.scalatest.{Suite, SuiteMixin}


trait WithSessionRepository extends SuiteMixin { this: Suite =>
  def withSessionRepository(test: FakeSessionRepository=>Any): Any = {
    val sessionRepository = new FakeSessionRepository()
    test(sessionRepository)
  }


}

private[identity] class FakeSessionRepository() extends SessionRepository {
  var sessions = Set.empty[SessionProjection]

  def getSessions:Set[SessionProjection] =
    sessions

  override def save(sessionProjection: SessionProjection): Unit =
    sessions+=sessionProjection
}
