package mixter.infra.repositories

import mixter.domain.identity.UserId
import mixter.infra.InMemoryEventStore
import org.scalatest.matchers.should.Matchers
import org.scalatest.wordspec.AnyWordSpec

class EventUserRepositorySpec extends AnyWordSpec with Matchers {
  "An EventUserRepository backed by an empty event store" should {
    "return an empty option when getting a user by its id" in {
      // Given
      val store = new InMemoryEventStore()
      val repository = new EventUserRepository(store)

      val maybeUser = repository.getById(USER_ID)
      //Then
      maybeUser should be(None)
    }
  }
  val USER_ID = UserId("user@example.localhost")
}
