package mixter.domain.subscription

import mixter.domain.SpyEventPublisherFixture
import mixter.domain.identity.UserId
import mixter.domain.message.MessageId
import mixter.domain.subscription.event.{FolloweeMessageQuacked, UserFollowed, UserUnfollowed}
import org.scalatest.{Matchers, WordSpec}

class SubscriptionSpec extends WordSpec with Matchers with SpyEventPublisherFixture {
  "Subscription" should {
    "raise UserFollowed when a user follows another user" in withSpyEventPublisher { implicit eventPublisher =>
      //Given

      //When
      Subscription.follow(FOLLOWER, FOLLOWEE)
      //Then
      val expected = UserFollowed(SUBSCRIPTION_ID)
      eventPublisher.publishedEvents should contain only expected
    }

    "raise UserUnfollowed when a follower unfollows a followee" in withSpyEventPublisher { implicit eventPublisher =>
      //Given
      val subscription = Subscription(UserFollowed(SUBSCRIPTION_ID))
      //When
      subscription.unfollow()
      //Then
      val expected = UserUnfollowed(SUBSCRIPTION_ID)
      eventPublisher.publishedEvents should contain only expected
    }

    "raise FolloweeMessageQuacked when notifying a follower" in withSpyEventPublisher { implicit eventPublisher =>
      //Given
      val subscription = Subscription(UserFollowed(SUBSCRIPTION_ID))
      val messageId = MessageId.generate()
      //When
      subscription.notifyFollower(messageId)
      //Then
      val expected = FolloweeMessageQuacked(SUBSCRIPTION_ID, messageId)
      eventPublisher.publishedEvents should contain only expected
    }
  }
  private val FOLLOWER = UserId("follower@example.localhost")
  private val FOLLOWEE = UserId("followee@example.localhost")
  private val SUBSCRIPTION_ID = SubscriptionId(FOLLOWER, FOLLOWEE)
}
