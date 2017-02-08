package domain.identity.event

import java.time.LocalDateTime

import domain.Event
import domain.identity.{SessionId, UserId}

case class UserConnected(sessionId: SessionId, userId: UserId, since: LocalDateTime) extends Event
