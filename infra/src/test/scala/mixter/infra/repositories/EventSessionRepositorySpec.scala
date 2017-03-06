package mixter.infra.repositories

import mixter.domain.identity.{SessionId, UserId}
import mixter.infra.InMemoryEventStore
import org.scalatest.{Matchers, WordSpec}

class EventSessionRepositorySpec extends WordSpec with Matchers {
  "A EventSessionRepository backed by an empty event store" should {
    "not find a Session aggregate by its Id" in {
      // Given
      val store = new InMemoryEventStore()
      val repository = new EventSessionRepository(store)

      val maybeSession = repository.getById(SESSION_ID)
      //Then
      maybeSession  should be(None)
    }
  }
  val SESSION_ID = SessionId()
  val USER_ID = UserId("user@example.localhost")
}
