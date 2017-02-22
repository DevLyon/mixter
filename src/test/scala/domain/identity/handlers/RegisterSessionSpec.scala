package domain.identity.handlers

import java.time.LocalDateTime

import domain.identity._
import domain.identity.event.UserConnected
import org.scalatest._

class RegisterSessionSpec extends WordSpec with Matchers with WithSessionRepository{

  "A RegisterSession handler" should {
    "save a connected SessionProjection when it receives UserConnected event" in withSessionRepository { sessionRepository =>
      // Given
      val userConnected = UserConnected(SessionId(), UserId("user@mixit.fr"), LocalDateTime.now())
      val handler = new RegisterSession(sessionRepository)
      // When
      handler.apply(userConnected)
      // Then
      val sessionProjection = SessionProjection(userConnected.sessionId, userConnected.userId, SessionStatus.CONNECTED)
      sessionRepository.getSessions should contain only(sessionProjection)
    }
  }


}
