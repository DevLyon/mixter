<?php

namespace Tests\Infrastructure\Subscriptions;

use App\Domain\Identity\UserId;
use App\Domain\Subscriptions\FollowerProjection;
use App\Domain\Subscriptions\SubscriptionId;
use App\Domain\Subscriptions\UpdateFollowers;
use App\Domain\Subscriptions\UserFollowed;
use App\Domain\Subscriptions\UserUnfollowed;
use App\Infrastructure\Subscriptions\FollowerProjectionRepository;
use Tests\Infrastructure\InMemoryProjectionStore;

class UpdateFollowersTest extends \PHPUnit_Framework_TestCase
{
    public function testWhenUserFollowed_ThenSaveFollower()
    {
        $followeeId = new UserId('florent@mix-it.fr');
        $userFollowed = new UserFollowed(new SubscriptionId(new UserId('clem@mix-it.fr'), $followeeId));
        $projectionStore = new InMemoryProjectionStore();
        $followerProjectionRepository = new FollowerProjectionRepository($projectionStore);
        $updateFollowers = new UpdateFollowers($followerProjectionRepository);

        $updateFollowers->handleUserFollowed($userFollowed);

        \Assert\that($followerProjectionRepository->getFollowersOf($followeeId))->count(1);
    }

    public function testWhenUserUnfollowed_ThenRemoveFollower()
    {
        $followeeId = new UserId('florent@mix-it.fr');
        $followerId = new UserId('clem@mix-it.fr');
        $userUnfollowed = new UserUnfollowed(new SubscriptionId($followerId, $followeeId));
        $followerProjection = new FollowerProjection($followerId, $followeeId);
        $projectionStore = new InMemoryProjectionStore(array($followerProjection->getSubscriptionId()->getId() => $followerProjection));
        $followerProjectionRepository = new FollowerProjectionRepository($projectionStore);
        $updateFollowers = new UpdateFollowers($followerProjectionRepository);

        $updateFollowers->handleUserUnfollowed($userUnfollowed);

        \Assert\that($followerProjectionRepository->getFollowersOf($followeeId))->count(0);
    }
}