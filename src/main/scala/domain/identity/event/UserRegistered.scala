package domain.identity.event

import domain.Event
import domain.identity.UserId

case class UserRegistered(userId: UserId) extends Event
