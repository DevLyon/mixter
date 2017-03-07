package mixter.domain.message

import mixter.domain.identity.UserId

trait TimelineMessageRepository {
  def save(message:TimelineMessageProjection):TimelineMessageProjection
  def getMessageOfUser(ownerId:UserId):Iterable[TimelineMessageProjection]
}
