package mixter.domain.subscription

import mixter.domain.AggregateId
import mixter.domain.identity.UserId

case class SubscriptionId(follower:UserId, followee:UserId) extends AggregateId
