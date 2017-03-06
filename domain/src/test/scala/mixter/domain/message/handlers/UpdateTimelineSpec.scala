package mixter.domain.message.handlers

import mixter.domain.identity.UserId
import mixter.domain.message.event.MessageQuacked
import mixter.domain.message.{MessageId, TimelineMessageProjection}
import org.scalatest.{Matchers, WordSpec}

class UpdateTimelineSpec extends WordSpec with Matchers with TimelineMessageRepositoryFixture {
  "A UpdateTimeline handler" should {
    "save a TimelineProjection when it receives a MessageQuacked" in withTimelineMessageRepository { timelineRepository =>
      // Given
      val messageId = MessageId.generate
      val messageQuacked = MessageQuacked(messageId, CONTENT, AUTHOR_ID)
      val handler = new UpdateTimeline(timelineRepository)

      // When
      handler(messageQuacked)

      // Then
      timelineRepository.getMessages should contain theSameElementsAs Seq(TimelineMessageProjection(AUTHOR_ID, AUTHOR_ID, CONTENT, messageId))
    }
  }

  private val AUTHOR_ID = UserId("author@example.localhost")
  private val USER_ID = UserId("someUser@example.localhost")
  private val CONTENT = "hello world"
}
