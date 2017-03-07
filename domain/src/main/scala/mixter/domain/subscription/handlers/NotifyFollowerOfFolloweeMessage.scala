package mixter.domain.subscription.handlers

import mixter.domain.EventPublisher
import mixter.domain.message.event.{MessageQuacked, MessageRequacked}
import mixter.domain.subscription.{FollowerRepository, SubscriptionId, SubscriptionRepository}

class NotifyFollowerOfFolloweeMessage(followerRepository: FollowerRepository,
                                      subscriptionRepository: SubscriptionRepository
                                     )(implicit val ep: EventPublisher) {

  def apply(messageQuacked: MessageQuacked): Unit = {
    for {
      follower <- followerRepository.getFollowers(messageQuacked.author)
      subscription <- subscriptionRepository.getById(SubscriptionId(follower, messageQuacked.author))
    } subscription.notifyFollower(messageQuacked.id)
  }
  def apply(messageRequacked: MessageRequacked): Unit = {
    for {
      follower <- followerRepository.getFollowers(messageRequacked.requacker)
      subscription <- subscriptionRepository.getById(SubscriptionId(follower, messageRequacked.requacker))
    } subscription.notifyFollower(messageRequacked.id)
  }

}
