package mixter.domain.subscription.handlers

import mixter.domain.identity.UserId
import mixter.domain.subscription.{FollowerRepositoryFixture, SubscriptionId}
import mixter.domain.subscription.event.{UserFollowed, UserUnfollowed}
import org.scalatest.wordspec.AnyWordSpec
import org.scalatest.matchers.should.Matchers

class UpdateFollowersSpec extends AnyWordSpec with Matchers with FollowerRepositoryFixture {
  "A UpdateFollowers handler" should {
    "add a follower to a user's followers list when receiving a UserFollowed event " in withFollowerRepository { repository =>
      // Given
      val handler = new UpdateFollowers(repository)
      // When
      handler(UserFollowedEvent)
      // Then
      repository.getFollowers(Followee) should contain only(Follower)
    }

    "remove a follower from a user's followers list when receiving a UserUnfollowed event" in withFollowerRepository { repository =>
      // Given
      val userUnfollowed = UserUnfollowed(ASubscriptionId)
      val handler = new UpdateFollowers(repository)
      handler(UserFollowedEvent)

      // When
      handler(userUnfollowed)

      // Then
      repository.getFollowers(Followee) should not contain Follower
    }

  }
  private val Follower = UserId("follower@example.localhost")
  private val Followee = UserId("followee@example.localhost")
  private val ASubscriptionId = SubscriptionId(Follower, Followee)
  private val UserFollowedEvent = UserFollowed(ASubscriptionId)
}
