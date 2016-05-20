package domain.identity

import domain.EventPublisher
import domain.identity.event.UserRegistered

object UserIdentity {
  def register(userId: UserId)(implicit ep:EventPublisher)=
    ep.publish(UserRegistered(userId))
}
