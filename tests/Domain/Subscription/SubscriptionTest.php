<?php

namespace Tests\Domain\Subscription;

use App\Domain\Identity\UserId;
use App\Domain\Subscriptions\Subscription;
use App\Domain\Subscriptions\SubscriptionId;
use App\Domain\Subscriptions\UserFollowed;
use Tests\Domain\FakeEventPublisher;

class SubscriptionTest extends \PHPUnit_Framework_TestCase
{
    public function testWhenFollowAnotherUser_ThenUserFollowedIsRaised()
    {
        $fakeEventPublisher = new FakeEventPublisher();
        $followeeId = new UserId('clem@mix-it.fr');
        $followerId = new UserId('jean@mix-it.fr');

        Subscription::followUser($fakeEventPublisher, $followerId, $followeeId);

        \Assert\that($fakeEventPublisher->events)->count(1);
        /** @var UserFollowed $userFollowed */
        $userFollowed = $fakeEventPublisher->events[0];
        \Assert\that($userFollowed->getSubscriptionId()->getFollowerId())->eq($followerId);
        \Assert\that($userFollowed->getSubscriptionId()->getFolloweeId())->eq($followeeId);
    }

    public function testWhenUnfollow_ThenUserUnfollowedIsRaised()
    {
        $fakeEventPublisher = new FakeEventPublisher();
        $followeeId = new UserId('clem@mix-it.fr');
        $followerId = new UserId('jean@mix-it.fr');
        $userFollowed = new UserFollowed(new SubscriptionId($followerId, $followeeId));
        $subscription = new Subscription(array($userFollowed));

        $subscription->unfollow($fakeEventPublisher);

        \Assert\that($fakeEventPublisher->events)->count(1);
        /** @var UserUnfollowed $userUnfollowed */
        $userUnfollowed = $fakeEventPublisher->events[0];
        \Assert\that($userUnfollowed)->isInstanceOf('App\Domain\Subscriptions\UserUnfollowed');
        \Assert\that($userUnfollowed->getSubscriptionId())->eq($userFollowed->getSubscriptionId());
    }
}