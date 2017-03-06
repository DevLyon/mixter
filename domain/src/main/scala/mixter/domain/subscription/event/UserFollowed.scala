package mixter.domain.subscription.event

import mixter.domain.subscription.SubscriptionId
import mixter.domain.{AggregateId, Event}

sealed trait SubscriptionEvent extends Event
case class UserFollowed(subscriptionId: SubscriptionId) extends SubscriptionEvent{
  override def id: AggregateId = subscriptionId
}
