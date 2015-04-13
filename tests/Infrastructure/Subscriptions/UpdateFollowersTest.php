<?php

namespace Tests\Infrastructure\Subscriptions;

use App\Domain\Identity\UserId;
use App\Domain\Subscriptions\SubscriptionId;
use App\Domain\Subscriptions\UserFollowed;
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
}