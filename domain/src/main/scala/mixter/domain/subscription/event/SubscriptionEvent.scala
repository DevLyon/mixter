package mixter.domain.subscription.event

import mixter.domain.message.MessageId
import mixter.domain.subscription.SubscriptionId
import mixter.domain.{AggregateId, Event}

sealed trait SubscriptionEvent extends Event
case class UserFollowed(subscriptionId: SubscriptionId) extends SubscriptionEvent{
  override def id: AggregateId = subscriptionId
}
case class UserUnfollowed(subscriptionId: SubscriptionId) extends SubscriptionEvent{
  override def id: AggregateId = subscriptionId
}
case class FolloweeMessageQuacked(subscriptionId: SubscriptionId, messageId: MessageId) extends SubscriptionEvent{
  override def id: AggregateId = subscriptionId
}