package domain.identity

import domain.SpyEventPublisher
import domain.identity.event.UserRegistered
import org.scalatest.{Matchers, WordSpec}

class UserIdentitySpec extends WordSpec with Matchers {

  val AN_EMAIL: String = "john@example.com"

  "A user identity" should {
    "raise a UserRegisteredEvent when it is registered with a UserId" in {
      val aUserId = UserId(AN_EMAIL)
      implicit val eventPublisher=new SpyEventPublisher()

      UserIdentity.register(aUserId)

      val expected = UserRegistered(aUserId)
      eventPublisher.publishedEvents should contain theSameElementsAs Seq(expected)
    }
  }
}
