package mixter.domain.subscription

trait SubscriptionRepository {
  def getById(subscriptionId: SubscriptionId): Option[Subscription]
}
