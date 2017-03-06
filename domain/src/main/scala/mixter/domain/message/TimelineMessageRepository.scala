package mixter.domain.message

trait TimelineMessageRepository {
  def save(message:TimelineMessageProjection):TimelineMessageProjection
}
