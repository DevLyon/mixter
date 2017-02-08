package domain.identity

import java.time.LocalDateTime

import domain.SpyEventPublisher
import domain.identity.event.{UserConnected, UserRegistered}
import org.scalatest.{Matchers, WordSpec}

class SessionSpec extends WordSpec with Matchers {
  val USER_ID = UserId("john@example.com")

  "A session" should {
    "be connected when a UserIdentity logs in" in {
      val userIdentity = UserIdentity(UserRegistered(USER_ID))
      implicit val eventPublisher=new SpyEventPublisher()

      userIdentity.logIn()

      eventPublisher.publishedEvents should matchPattern {
        case Seq(UserConnected(_:SessionId, USER_ID, _:LocalDateTime)) =>
      }
    }
  }
}