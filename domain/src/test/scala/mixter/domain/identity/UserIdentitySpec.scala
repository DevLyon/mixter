package mixter.domain.identity

import mixter.domain.SpyEventPublisherFixture
import mixter.domain.identity.event.UserRegistered
import org.scalatest.{Matchers, WordSpec}

class UserIdentitySpec extends WordSpec with Matchers with SpyEventPublisherFixture {

  val AN_EMAIL: String = "john@example.com"

  "A user identity" should {
    "raise a UserRegisteredEvent when it is registered with a UserId" in withSpyEventPublisher { implicit eventPublisher=>
      val aUserId = UserId(AN_EMAIL)

      UserIdentity.register(aUserId)

      val expected = UserRegistered(aUserId)
      eventPublisher.publishedEvents should contain theSameElementsAs Seq(expected)
    }
  }
}
