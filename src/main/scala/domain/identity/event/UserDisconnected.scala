package domain.identity.event

import domain.Event
import domain.identity.{SessionId, UserId}

case class UserDisconnected(sessionId:SessionId, userId:UserId) extends Event
