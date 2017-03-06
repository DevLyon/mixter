package mixter.infra

import mixter.domain.identity.{SessionId, SessionProjection}
import org.scalatest.{Matchers, WordSpec}

class InMemorySessionProjectionRepositorySpec extends WordSpec with Matchers {
  "An empty in memory SessionProjectionRepository" should {
    "return an empty option when getting a session projection by id " in {
      val repository = new InMemorySessionProjectionRepository()
      val sessionId:SessionId = SessionId()
      val actual:Option[SessionProjection] = repository.getById(sessionId)

      actual should be(None)
    }
  }
}
