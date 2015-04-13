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
                $existingFollower->getSubscriptionId()->getId() => $existingFollower,
                $otherFollower->getSubscriptionId()->getId() => $otherFollower));
        $followerProjectionRepository = new FollowerProjectionRepository($projectionStore);

        $followers = $followerProjectionRepository->getFollowersOf($followeeId);

        \Assert\that($followers)->count(1);
        /** @var FollowerProjection $follower */
        $follower = $followers[0];
        \Assert\that($follower->getFollowerId())->eq($followerId);
        \Assert\that($follower->getFolloweeId())->eq($followeeId);
    }

    public function testGivenNoFollowersExist_WhenGetByUserId_ThenReturnsEmptyArray()
    {
        $projectionStore = new InMemoryProjectionStore();
        $followerProjectionRepository = new FollowerProjectionRepository($projectionStore);

        $followers = $followerProjectionRepository->getFollowersOf(new UserId('clem@mix-it.fr'));

        \Assert\that($followers)->count(0);
    }

    public function testWhenSaveFollower_ThenStoresIt()
    {
        $projectionStore = new InMemoryProjectionStore();
        $followerProjectionRepository = new FollowerProjectionRepository($projectionStore);
        $followerProjection = new FollowerProjection(new UserId('clem@mix-it.fr'), new UserId('florent@mix-it.fr'));

        $followerProjectionRepository->save($followerProjection);

        \Assert\that($projectionStore->getAll('App\Domain\Subscriptions\FollowerProjection'))->count(1);
    }

    public function testWhenRemoveFollower_ThenRemoveFromStore()
    {
        $followeeId = new UserId("jean@mix-it.fr");
        $followerId = new UserId("clem@mix-it.fr");
        $existingFollower = new FollowerProjection($followerId, $followeeId);
        $projectionStore = new InMemoryProjectionStore(array($existingFollower->getSubscriptionId()->getId() => $existingFollower));
        $followerProjectionRepository = new FollowerProjectionRepository($projectionStore);
        $followerProjection = new FollowerProjection($followerId, $followeeId);

        $followerProjectionRepository->remove($followerProjection);

        \Assert\that($projectionStore->getAll('App\Domain\Subscriptions\FollowerProjection'))->count(0);
    }
}