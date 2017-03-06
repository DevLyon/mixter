package mixter.infra

import mixter.domain.identity.{SessionId, SessionProjection, UserId}
import org.scalatest.OptionValues._
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
  "An memory SessionProjectionRepository with a saved SessionProjection" should {
    "return the saved SessionProjection when getting a session projection by its id " in {
      //Given
      val sessionId:SessionId = SessionId()
      val sessionProjection = SessionProjection(sessionId, UserId("user@example.localhost"))
      val repository = new InMemorySessionProjectionRepository()
      repository.save(sessionProjection)

      //When
      val actual:Option[SessionProjection] = repository.getById(sessionId)

      //Then
      actual.value should be(sessionProjection)
    }
  }
}
