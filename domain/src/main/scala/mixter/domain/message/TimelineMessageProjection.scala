package mixter.domain.message

import mixter.domain.identity.UserId

case class TimelineMessageProjection(ownerId: UserId, authorId: UserId, content: String, messageId: MessageId)
