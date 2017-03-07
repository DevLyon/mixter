package mixter.domain.message.handlers

import mixter.domain.message.event.{MessageDeleted, MessageQuacked}
import mixter.domain.message.{TimelineMessageProjection, TimelineMessageRepository}

class UpdateTimeline(repository: TimelineMessageRepository) {
  def apply(event: MessageQuacked): Unit = {
    repository.save(TimelineMessageProjection(event.author, event.author, event.message, event.id))
  }
  def apply(event: MessageDeleted): Unit = {
    repository.delete(event.id)
  }
}

