package mixter.infra.repositories

import java.time.LocalDateTime

import mixter.domain.identity.event.UserConnected
import mixter.domain.identity.{Session, SessionId, UserId}
import mixter.infra.InMemoryEventStore
import org.scalatest.OptionValues._
import org.scalatest.{Matchers, WordSpec}

class EventSessionRepositorySpec extends WordSpec with Matchers {
  "An EventSessionRepository backed by an empty event store" should {
    "not find a Session aggregate by its Id" in {
      // Given
      val store = new InMemoryEventStore()
      val repository = new EventSessionRepository(store)

      val maybeSession = repository.getById(SESSION_ID)
      //Then
      maybeSession  should be(None)
    }
  }

  "An EventSessionRepository backed by an event store with a UserConnected event" should {
    "find a Session aggregate by its id" in {
      // Given
      val userConnected = UserConnected(SESSION_ID, USER_ID,LocalDateTime.now())
      val store = new InMemoryEventStore()
      store.store(userConnected)
      val repository = new EventSessionRepository(store)

      val maybeSession = repository.getById(SESSION_ID)
      //Then
      maybeSession.value should ===(Session(userConnected, Seq.empty))
    }
  }
  val SESSION_ID = SessionId()
  val USER_ID = UserId("user@example.localhost")
}
