package mixter.domain.subscription.handlers

import mixter.domain.identity.UserId
import mixter.domain.message.MessageId
import mixter.domain.message.event.{MessageQuacked, MessageRequacked}
import mixter.domain.subscription._
import mixter.domain.subscription.event.{FolloweeMessageQuacked, UserFollowed}
import mixter.domain.{SpyEventPublisher, SpyEventPublisherFixture}
import org.scalatest.{Matchers, WordSpec}

class NotifyFollowerOfFolloweeMessageSpec extends WordSpec with Matchers with SubscriptionRepositoryFixture with FollowerRepositoryFixture with SpyEventPublisherFixture {


  "A NotifyFollowerOfFolloweeMessage" should {
    "notify followers when a followee publishes a message" in withFixtures {
      (subscriptionRepository, followerRepository, eventPublisher) =>
        // Given
        subscriptionRepository.add(Subscription(UserFollowed(ASubscriptionId)))
        followerRepository.saveFollower(AuthorId, AFollowerId)
        val handler = new NotifyFollowerOfFolloweeMessage(followerRepository, subscriptionRepository)(eventPublisher)
        val messageQuacked = MessageQuacked(AMessageId, Content, AuthorId)

        // When
        handler(messageQuacked)

        // Then
        val followeeMessagePublished = FolloweeMessageQuacked(ASubscriptionId, AMessageId)
        eventPublisher.publishedEvents should contain only followeeMessagePublished
    }

    "notify followers when a followee requacks a message" in withFixtures {
      (subscriptionRepository, followerRepository, eventPublisher) =>
        // Given
        subscriptionRepository.add(Subscription(UserFollowed(BSubscriptionId)))
        followerRepository.saveFollower(RequackerId, AFollowerId)
        val handler = new NotifyFollowerOfFolloweeMessage(followerRepository, subscriptionRepository)(eventPublisher)
        val messageQuacked = MessageRequacked(AMessageId, RequackerId, AuthorId, Content)

        // When
        handler(messageQuacked)

        // Then
        val followeeMessagePublished = FolloweeMessageQuacked(BSubscriptionId, AMessageId)
        eventPublisher.publishedEvents should contain only followeeMessagePublished
    }

  }

  private val Content = "Content"
  private val AuthorId = new UserId("author@example.localhost")
  private val RequackerId = new UserId("requacker@example.localhost")
  private val AFollowerId = new UserId("follower@example.localhost")
  private val AMessageId = MessageId.generate()
  private val ASubscriptionId = SubscriptionId(AFollowerId, AuthorId)
  private val BSubscriptionId = SubscriptionId(AFollowerId, RequackerId)

  private def withFixtures(blk: (FakeSubscriptionRepository, FakeFollowerRepository, SpyEventPublisher) => Any) = {
    withSubscriptionRepository { _ =>
      withFollowerRepository { _ =>
        withSpyEventPublisher { _ =>
          blk(_, _, _)
        }
      }
    }
  }
}