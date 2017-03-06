package mixter.domain.subscription

import mixter.domain.EventPublisher
import mixter.domain.identity.UserId
import mixter.domain.subscription.event.UserFollowed

class Subscription

object Subscription {
  def follow(follower: UserId, followee: UserId)(implicit ep:EventPublisher): Unit = {
    ep.publish(UserFollowed(SubscriptionId(follower, followee)))
  }
}
