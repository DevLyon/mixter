package mixter.infra.repositories

import mixter.domain.identity.event.UserRegistered
import mixter.domain.identity.{UserId, UserIdentity}
import mixter.infra.InMemoryEventStore
import org.scalatest.{Matchers, WordSpec}
import org.scalatest.OptionValues._

class EventUserRepositorySpec extends WordSpec with Matchers {
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

  "An EventUserRepository backed by an event store with a UserRegistered event" should {
    "find a User aggregate by its id" in {
      // Given
      val userRegistered = UserRegistered(USER_ID)
      val store = new InMemoryEventStore()
      store.store(userRegistered)
      val repository = new EventUserRepository(store)

      val maybeUser = repository.getById(USER_ID)
      //Then
      maybeUser.value should ===(UserIdentity(userRegistered))
    }
  }
  val USER_ID = UserId("user@example.localhost")
}
