package mixter.domain.subscription

import mixter.domain.identity.UserId

trait FollowerRepository {

  def getFollowers(authorId: UserId): Set[UserId]

  def saveFollower(followee: UserId, follower: UserId): Unit

  def removeFollower(followee: UserId, follower: UserId): Unit
}
