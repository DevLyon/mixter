package mixter.domain.subscription

import mixter.domain.SpyEventPublisherFixture
import mixter.domain.identity.UserId
import mixter.domain.subscription.event.UserFollowed
import org.scalatest.{Matchers, WordSpec}

class SubscriptionSpec extends WordSpec with Matchers with SpyEventPublisherFixture {
  "Subscription" should {
    "raise UserFollowed when a user follows another user" in withSpyEventPublisher { implicit eventPublisher =>
      //Given

      //When
      Subscription.follow(FOLLOWER, FOLLOWEE)
      //Then
      val expected = UserFollowed(SubscriptionId(FOLLOWER, FOLLOWEE))
      eventPublisher.publishedEvents should contain only expected
    }
  }
  private val FOLLOWER = UserId("follower@example.localhost")
  private val FOLLOWEE = UserId("followee@example.localhost")
}
