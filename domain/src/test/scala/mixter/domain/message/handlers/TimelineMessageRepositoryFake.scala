package mixter.domain.message.handlers

import mixter.domain.message.{TimelineMessageProjection, TimelineMessageRepository}
import org.scalatest.{Suite, SuiteMixin}

trait TimelineMessageRepositoryFixture extends SuiteMixin {
  this: Suite =>
  def withTimelineMessageRepository(test: TimelineMessageRepositoryFake => Any): Any = {
    val sessionRepository = new TimelineMessageRepositoryFake()
    test(sessionRepository)
  }

}

class TimelineMessageRepositoryFake extends TimelineMessageRepository {
  var messages: Seq[TimelineMessageProjection] = Seq.empty

  def getMessages: Seq[TimelineMessageProjection] = messages

  override def save(message:TimelineMessageProjection):TimelineMessageProjection = {
    messages = messages :+ message
    message
  }
}
