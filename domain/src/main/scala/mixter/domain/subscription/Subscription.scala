package mixter.domain.subscription

import mixter.domain.EventPublisher
import mixter.domain.identity.UserId
import mixter.domain.message.MessageId
import mixter.domain.subscription.event.{FolloweeMessageQuacked, SubscriptionEvent, UserFollowed, UserUnfollowed}

case class Subscription(initialEvent: UserFollowed, history: Seq[SubscriptionEvent]=Seq.empty) {
  import Subscription._

  private val projection = {
    val seed = DecisionProjection.of(initialEvent)
    history.foldLeft(seed)((acc, evt) => acc(evt))
  }

  def unfollow()(implicit ep: EventPublisher): Unit = {
    ep.publish(UserUnfollowed(projection.subscriptionId))
  }

  def notifyFollower(messageId: MessageId)(implicit ep: EventPublisher): Unit = {
    if(projection.isFollowing){
      ep.publish(FolloweeMessageQuacked(projection.subscriptionId, messageId))
    }
  }
}

object Subscription {
  def follow(follower: UserId, followee: UserId)(implicit ep: EventPublisher): Unit = {
    ep.publish(UserFollowed(SubscriptionId(follower, followee)))
  }

  case class DecisionProjection(subscriptionId: SubscriptionId, isFollowing:Boolean=true){
    def apply(evt:SubscriptionEvent)=evt match {
      case UserUnfollowed(_) => copy(isFollowing=false)
      case _ => this
    }
  }

  object DecisionProjection {
    def of(userFollowed: UserFollowed) = DecisionProjection(userFollowed.subscriptionId)
  }

}
