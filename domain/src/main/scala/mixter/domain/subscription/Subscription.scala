package mixter.domain.subscription

import mixter.domain.EventPublisher
import mixter.domain.identity.UserId
import mixter.domain.subscription.event.{UserFollowed, UserUnfollowed}


case class Subscription(initialEvent:UserFollowed) {
  import Subscription._
  private val projection = DecisionProjection.of(initialEvent)

  def unfollow()(implicit ep:EventPublisher):Unit = {
    ep.publish(UserUnfollowed(projection.subscriptionId))
  }
}

object Subscription {
  def follow(follower: UserId, followee: UserId)(implicit ep:EventPublisher): Unit = {
    ep.publish(UserFollowed(SubscriptionId(follower, followee)))
  }

  case class DecisionProjection(subscriptionId: SubscriptionId)
  object DecisionProjection{
    def of(userFollowed: UserFollowed)= DecisionProjection(userFollowed.subscriptionId)
  }
}

