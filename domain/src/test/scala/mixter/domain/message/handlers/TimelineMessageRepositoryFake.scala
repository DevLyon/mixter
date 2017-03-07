package mixter.domain.message.handlers

import mixter.domain.identity.UserId
import mixter.domain.message.{MessageId, TimelineMessageProjection, TimelineMessageRepository}
import org.scalatest.{Suite, SuiteMixin}

trait TimelineMessageRepositoryFixture extends SuiteMixin {
  this: Suite =>
  def withTimelineMessageRepository(test: TimelineMessageRepositoryFake => Any): Any = {
    val sessionRepository = new TimelineMessageRepositoryFake()
    test(sessionRepository)
  }

}

class TimelineMessageRepositoryFake extends TimelineMessageRepository {
  private[handlers] var messages: Seq[TimelineMessageProjection] = Seq.empty
  private[handlers] var deletedMessageIds:Set[MessageId] = Set.empty

  def getMessages: Seq[TimelineMessageProjection] = messages

  override def save(message: TimelineMessageProjection): TimelineMessageProjection = {
    messages = messages :+ message
    message
  }

  override def delete(messageId: MessageId): Unit = {
    deletedMessageIds += messageId
  }

  override def getMessageOfUser(ownerId: UserId): Iterable[TimelineMessageProjection] = ???
}
