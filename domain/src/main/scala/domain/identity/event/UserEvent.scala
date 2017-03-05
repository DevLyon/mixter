package domain.identity.event

import java.time.LocalDateTime

import domain.Event
import domain.identity.{SessionId, UserId}

sealed trait UserEvent extends Event
case class UserRegistered(id: UserId) extends UserEvent{}
sealed trait UserSessionEvent extends UserEvent
case class UserConnected(sessionId: SessionId, id: UserId, since: LocalDateTime) extends UserSessionEvent
case class UserDisconnected(sessionId:SessionId, id:UserId) extends UserSessionEvent