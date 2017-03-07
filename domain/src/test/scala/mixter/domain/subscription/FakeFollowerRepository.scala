package mixter.domain.subscription

import mixter.domain.identity.UserId
import org.scalatest.{Suite, SuiteMixin}

private[subscription] trait FollowerRepositoryFixture extends SuiteMixin {
  this: Suite =>
  def withFollowerRepository(test: FakeFollowerRepository => Any): Any = {
    val sessionRepository = new FakeFollowerRepository()
    test(sessionRepository)
  }
}

private[subscription] class FakeFollowerRepository extends FollowerRepository {
  private var subscriptions = Map.empty[UserId, Set[UserId]].withDefaultValue(Set.empty)

  override def getFollowers(authorId: UserId): Set[UserId] = subscriptions(authorId)

  override def saveFollower(followee: UserId, follower: UserId): Unit =
    subscriptions += followee -> (subscriptions(followee) + follower)

  override def removeFollower(followee: UserId, follower: UserId): Unit =
    subscriptions += followee -> (subscriptions(followee) - follower)
}