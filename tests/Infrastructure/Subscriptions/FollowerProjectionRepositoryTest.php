<?php

namespace Infrastructure\Subscriptions;

use App\Domain\Identity\UserId;
use App\Domain\Subscriptions\FollowerProjection;
use App\Infrastructure\Subscriptions\FollowerProjectionRepository;
use Tests\Infrastructure\InMemoryProjectionStore;

class FollowerProjectionRepositoryTest extends \PHPUnit_Framework_TestCase
{
    public function testGivenExistingFollower_WhenGetByUserId_ThenReturnsOnlyFollowersOfUserId()
    {
        $followeeId = new UserId("jean@mix-it.fr");
        $followerId = new UserId("clem@mix-it.fr");
        $existingFollower = new FollowerProjection($followerId, $followeeId);
        $otherFollower = new FollowerProjection($followerId, new UserId('someoneelse@mix-it.fr'));
        $projectionStore = new InMemoryProjectionStore(
            array(
                $existingFollower->getFolloweeId()->getId() => $existingFollower,
                $otherFollower->getFolloweeId()->getId() => $otherFollower));
        $followerProjectionRepository = new FollowerProjectionRepository($projectionStore);

        $followers = $followerProjectionRepository->getFollowersOf($followeeId);

        \Assert\that($followers)->count(1);
        /** @var FollowerProjection $follower */
        $follower = $followers[0];
        \Assert\that($follower->getFollowerId())->eq($followerId);
        \Assert\that($follower->getFolloweeId())->eq($followeeId);
    }
}