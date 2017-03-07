package mixter.domain.subscription.handlers

import mixter.domain.EventPublisher
import mixter.domain.identity.UserId
import mixter.domain.message.MessageId
import mixter.domain.message.event.{MessageQuacked, MessageRequacked}
import mixter.domain.subscription.{FollowerRepository, SubscriptionId, SubscriptionRepository}

class NotifyFollowerOfFolloweeMessage(followerRepository: FollowerRepository,
                                      subscriptionRepository: SubscriptionRepository
                                     )(implicit val ep: EventPublisher) {

  def apply(event: MessageQuacked): Unit = {
    notifyFollowers(event.author, event.id)
  }

  def apply(event: MessageRequacked): Unit = {
    notifyFollowers(event.requacker, event.id)
  }

  private def notifyFollowers(followee:UserId, messageId:MessageId) = {
    for {
      follower <- followerRepository.getFollowers(followee)
      subscription <- subscriptionRepository.getById(SubscriptionId(follower, followee))
    } subscription.notifyFollower(messageId)
  }
}
