package mixter.domain.identity

import mixter.domain.SpyEventPublisher
import mixter.domain.identity.event.UserRegistered
import org.scalatest.matchers.should.Matchers
import org.scalatest.wordspec.AnyWordSpec

class UserIdentitySpec extends AnyWordSpec with Matchers {

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
