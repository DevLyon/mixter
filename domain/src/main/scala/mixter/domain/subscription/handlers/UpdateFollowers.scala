package mixter.domain.subscription.handlers

import mixter.domain.subscription.FollowerRepository
import mixter.domain.subscription.event.{UserFollowed, UserUnfollowed}

class UpdateFollowers(repository: FollowerRepository) {
  def apply(event:UserFollowed):Unit ={
    repository.saveFollower(event.subscriptionId.followee, event.subscriptionId.follower)
  }

  def apply(event:UserUnfollowed):Unit ={
    repository.removeFollower(event.subscriptionId.followee, event.subscriptionId.follower)
  }
}
