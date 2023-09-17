package mixter.domain.message.handlers

import mixter.domain.identity.UserId
import mixter.domain.message.event.{MessageDeleted, MessageQuacked}
import mixter.domain.message.{MessageId, TimelineMessageProjection}
import org.scalatest.matchers.should.Matchers
import org.scalatest.wordspec.AnyWordSpec

class UpdateTimelineSpec extends AnyWordSpec with Matchers with TimelineMessageRepositoryFixture {
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
    "remove all TimelineMessageProjection for the message id when it receives a MessageDeleted" in withTimelineMessageRepository { timelineRepository =>
      // Given
      val messageId = MessageId.generate
      val messageQuacked = MessageQuacked(messageId, CONTENT, AUTHOR_ID)
      val messageDeleted= MessageDeleted(messageId)
      val handler = new UpdateTimeline(timelineRepository)
      handler(messageQuacked)

      // When
      handler(messageDeleted)
      // Then
      timelineRepository.deletedMessageIds should contain only messageId
    }
  }

  private val AUTHOR_ID = UserId("author@example.localhost")
  private val USER_ID = UserId("someUser@example.localhost")
  private val CONTENT = "hello world"
}
