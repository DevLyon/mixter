package mixter.infra.repositories

import mixter.domain.identity.UserId
import mixter.domain.subscription.FollowerRepository

class InMemoryFollowerRepository extends FollowerRepository {
  private var followers = Map.empty[UserId, Set[UserId]].withDefaultValue(Set.empty)

  override def getFollowers(authorId: UserId): Set[UserId] = followers(authorId)

  override def saveFollower(followee: UserId, follower: UserId): Unit =
    followers+= followee -> (followers(followee)+follower)

  override def removeFollower(followee: UserId, follower: UserId): Unit =
    followers+= followee -> (followers(followee)-follower)
}
