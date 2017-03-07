package mixter.domain.subscription.handlers

import mixter.domain.identity.UserId
import mixter.domain.subscription.event.UserFollowed
import mixter.domain.subscription.{FollowerRepositoryFixture, SubscriptionId}
import org.scalatest.wordspec.AnyWordSpec
import org.scalatest.matchers.should.Matchers

class UpdateFollowersSpec extends AnyWordSpec with Matchers with FollowerRepositoryFixture {
  "A UpdateFollowers handler" should {
    "add a follower to a user's followers list when receiving a UserFollowed event " in withFollowerRepository { repository =>
      // Given
      val userFollowed = UserFollowed(SubscriptionId(Follower, Followee))
      val handler = new UpdateFollowers(repository)
      // When
      handler(userFollowed)
      // Then
      repository.getFollowers(Followee) should contain only(Follower)
    }
  }
  private val Follower = UserId("follower@example.localhost")
  private val Followee = UserId("followee@example.localhost")
}
