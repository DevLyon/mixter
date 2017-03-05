package mixter.domain.identity

import java.time.LocalDateTime

import mixter.domain.SpyEventPublisher
import mixter.domain.identity.event.{UserConnected, UserDisconnected, UserRegistered}
import org.scalatest.{Matchers, WordSpec}

class SessionSpec extends WordSpec with Matchers {
  val USER_ID = UserId("john@example.com")
  val SESSION_ID = SessionId()

  "A session" should {
    "be connected when a UserIdentity logs in" in {
      val userIdentity = UserIdentity(UserRegistered(USER_ID))
      implicit val eventPublisher=new SpyEventPublisher()

      userIdentity.logIn()

      eventPublisher.publishedEvents should matchPattern {
        case Seq(UserConnected(SessionId(_), USER_ID, _:LocalDateTime)) =>
      }
    }
  }
  private val userConnected = UserConnected(SESSION_ID, USER_ID, LocalDateTime.now())
  private val userDisconnected = UserDisconnected(SESSION_ID, USER_ID)

  "A session" should{
    "raise UserDisconnected when logging out" in {
      // Given
      val session: Session= Session(userConnected)
      implicit val eventPublisher=new SpyEventPublisher()
      // When
      session.logout()
      // Then
      eventPublisher.publishedEvents should matchPattern {
        case Seq(`userDisconnected`) =>
      }
    }
  }
  "A disconnected session" should{
    "not raise UserDisconnected when logging out" in {
      // Given
      val session: Session= Session(userConnected, Seq(userDisconnected))
      implicit val eventPublisher=new SpyEventPublisher()
      // When
      session.logout()
      // Then
      eventPublisher.publishedEvents shouldBe empty
    }
  }
}