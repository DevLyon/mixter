package domain.identity

case class SessionProjection(sessionId:SessionId, userId:UserId, status:SessionStatus=SessionStatus.CONNECTED)
