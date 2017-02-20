package domain.identity.event

import java.time.LocalDateTime

import domain.Event
import domain.identity.{SessionId, UserId}

sealed trait UserEvent extends Event
case class UserRegistered(userId: UserId) extends UserEvent
sealed trait UserSessionEvent extends UserEvent
case class UserConnected(sessionId: SessionId, userId: UserId, since: LocalDateTime) extends UserSessionEvent
case class UserDisconnected(sessionId:SessionId, userId:UserId) extends UserSessionEvent