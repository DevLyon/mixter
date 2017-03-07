package mixter.domain.subscription.handlers

import mixter.domain.identity.UserId
import mixter.domain.message.MessageId
import mixter.domain.message.event.MessageQuacked
import mixter.domain.subscription._
import mixter.domain.subscription.event.{FolloweeMessageQuacked, UserFollowed}
import mixter.domain.{SpyEventPublisher, SpyEventPublisherFixture}
import org.scalatest.wordspec.AnyWordSpec
import org.scalatest.matchers.should.Matchers

class NotifyFollowerOfFolloweeMessageSpec extends AnyWordSpec with Matchers with SubscriptionRepositoryFixture with FollowerRepositoryFixture with SpyEventPublisherFixture {

  "A NotifyFollowerOfFolloweeMessage" should {
    "notify followers when a followee publishes a message" in withFixtures {
      (subscriptionRepository, followerRepository, eventPublisher) =>
        // Given
        subscriptionRepository.add(Subscription(UserFollowed(ASubscriptionId)))
        followerRepository.saveFollower(ASubscriptionId.followee, ASubscriptionId.follower)
        val handler = new NotifyFollowerOfFolloweeMessage(followerRepository, subscriptionRepository)(eventPublisher)
        val messageQuacked = MessageQuacked(AMessageId, Content, ASubscriptionId.followee)

        // When
        handler(messageQuacked)

        // Then
        val followeeMessagePublished = FolloweeMessageQuacked(ASubscriptionId, AMessageId)
        eventPublisher.publishedEvents should contain only followeeMessagePublished
    }
  }

  private val Content = "Content"
  private val AMessageId = MessageId.generate()
  private val ASubscriptionId = SubscriptionId(new UserId("follower@example.localhost"), new UserId("followee@example.localhost"))

  private def withFixtures(blk: (FakeSubscriptionRepository, FakeFollowerRepository, SpyEventPublisher) => Any) = {
    withSubscriptionRepository { sr =>
      withFollowerRepository { fr =>
        withSpyEventPublisher { sep =>
          blk(sr, fr, sep)
        }
      }
    }
  }
}