package mixter.infra

import mixter.domain.identity.{SessionId, SessionProjection, SessionStatus, UserId}
import org.scalatest.OptionValues._
import org.scalatest.{Matchers, WordSpec}

class InMemorySessionProjectionRepositorySpec extends WordSpec with Matchers {
  "An empty in memory SessionProjectionRepository" should {
    "return an empty option when getting a session projection by id " in {
      val repository = new InMemorySessionProjectionProjectionRepository()
      val sessionId:SessionId = SessionId()
      val actual:Option[SessionProjection] = repository.getById(sessionId)

      actual should be(None)
    }
  }
  "An in memory SessionProjectionRepository with a saved SessionProjection" should {
    "return the saved SessionProjection when getting a session projection by its id " in {
      //Given
      val sessionId:SessionId = SessionId()
      val sessionProjection = SessionProjection(sessionId, UserId("user@example.localhost"))
      val repository = new InMemorySessionProjectionProjectionRepository()
      repository.save(sessionProjection)

      //When
      val actual:Option[SessionProjection] = repository.getById(sessionId)

      //Then
      actual.value should be(sessionProjection)
    }

    "return the updated SessionProjection after it is replaced by an updated projection" in {
      //Given
      val sessionId:SessionId = SessionId()
      val disconnected = SessionProjection(sessionId, UserId("user@example.localhost"), SessionStatus.DISCONNECTED)
      val connected = SessionProjection(sessionId, UserId("user@example.localhost"), SessionStatus.CONNECTED)
      val repository = new InMemorySessionProjectionProjectionRepository()
      repository.save(disconnected)

      //When
      repository.replaceBy(connected)

      //Then
      repository.getById(sessionId).value should be(connected)
    }
  }
  "An in memory SessionProjectionRepository with a saved disconnected SessionProjection" should {
    "return an empty option when getting the session projection by id" in {
      //Given
      val sessionId:SessionId = SessionId()
      val sessionProjection = SessionProjection(sessionId, UserId("user@example.localhost"), SessionStatus.DISCONNECTED)
      val repository = new InMemorySessionProjectionProjectionRepository()
      repository.save(sessionProjection)

      //When
      val actual:Option[SessionProjection] = repository.getById(sessionId)

      //Then
      actual should be(None)
    }
  }
}
