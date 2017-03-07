package mixter.domain.subscription

import org.scalatest.{Suite, SuiteMixin}

private[subscription] trait SubscriptionRepositoryFixture extends SuiteMixin {
  this: Suite =>
  def withSubscriptionRepository(test: FakeSubscriptionRepository => Any): Any = {
    val sessionRepository = new FakeSubscriptionRepository()
    test(sessionRepository)
  }
}

private[subscription] class FakeSubscriptionRepository extends SubscriptionRepository {
  private var subscriptions = Set.empty[Subscription]

  def add(subscription: Subscription): Unit = subscriptions += subscription

  override def getById(subscriptionId: SubscriptionId): Option[Subscription] =
    subscriptions.find(_.initialEvent.subscriptionId==subscriptionId)
}