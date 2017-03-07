package mixter.infra.repositories

import mixter.domain.identity.UserId
import mixter.domain.message.{MessageId, TimelineMessageProjection}
import org.scalatest.matchers.should.Matchers
import org.scalatest.wordspec.AnyWordSpec

class InMemoryTimelineRepositorySpec extends AnyWordSpec with Matchers {
  "An InMemoryTimelineRepository" should {
    "return a message for a usser when a message for this user was saved" in {
      val repository = new InMemoryTimelineRepository()
      val messageId=MessageId.generate()
      val timelineProjection = TimelineMessageProjection(OwnerId, AuthorId, "Hello", messageId)
      repository.save(timelineProjection)

      val messages= repository.getMessageOfUser(OwnerId).toSeq

      messages should contain(timelineProjection)
    }

    "return a message for an owner when 2 messages have been saved for 2 owners" in {
      val repository = new InMemoryTimelineRepository()
      val messageId=MessageId.generate()
      val timelineProjection = TimelineMessageProjection(OwnerId, AuthorId, "Hello", messageId)
      val otherTimelineProjection = TimelineMessageProjection(AliceId, AuthorId, "Hello", messageId)
      repository.save(timelineProjection)
      repository.save(otherTimelineProjection)

      val messages= repository.getMessageOfUser(OwnerId).toSeq

      messages should contain only timelineProjection
    }

    "return a single message when a message is saved twice for the same owner" in {
      val repository = new InMemoryTimelineRepository()
      val messageId=MessageId.generate()
      val timelineProjection = TimelineMessageProjection(OwnerId, AuthorId, "Hello", messageId)
      repository.save(timelineProjection)
      repository.save(timelineProjection)

      val messages= repository.getMessageOfUser(OwnerId).toSeq

      messages should contain theSameElementsAs Seq(timelineProjection)
    }

    "delete a message for all users when a message is deleted" in {
      val repository = new InMemoryTimelineRepository()
      val messageId=MessageId.generate()
      repository.save(TimelineMessageProjection(OwnerId, AuthorId, "Hello", messageId))
      repository.save(TimelineMessageProjection(AliceId, AuthorId, "Hello", messageId))

      repository.delete(messageId)

      repository.getMessageOfUser(OwnerId) shouldBe empty
      repository.getMessageOfUser(AliceId) shouldBe empty
    }
  }

  private val OwnerId = UserId("owner@example.localhost")
  private val AuthorId = UserId("author@example.localhost")
  private val AliceId = UserId("alice@example.localhost")
}
