package mixter.domain.identity.event

import java.time.LocalDateTime

import mixter.domain.identity.{SessionId, UserId}
import mixter.domain.{AggregateId, Event}

sealed trait UserEvent extends Event
case class UserRegistered(id: UserId) extends UserEvent

sealed trait UserSessionEvent extends Event
case class UserConnected(sessionId: SessionId, userId: UserId, since: LocalDateTime) extends UserSessionEvent{
  override def id: AggregateId = sessionId
}
case class UserDisconnected(sessionId:SessionId, userId:UserId) extends UserSessionEvent{
  override def id: AggregateId = sessionId
}