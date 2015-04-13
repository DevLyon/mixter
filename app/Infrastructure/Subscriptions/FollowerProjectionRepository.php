<?php

namespace App\Infrastructure\Subscriptions;

use App\Domain\Identity\UserId;
use App\Domain\Subscriptions\FollowerProjection;
use App\Domain\Subscriptions\IFollowerProjectionRepository;
use App\Infrastructure\IProjectionStore;

class FollowerProjectionRepository implements IFollowerProjectionRepository
{
    /**
     * @var IProjectionStore
     */
    private $projectionStore;

    public function __construct(IProjectionStore $projectionStore)
    {
        $this->projectionStore = $projectionStore;
    }

    public function getFollowersOf(UserId $followeeId)
    {
        return array_filter(
            $this->projectionStore->getAll('App\Domain\Subscriptions\FollowerProjection'),
            function(FollowerProjection $item) use($followeeId) {
                return $item->getFolloweeId() == $followeeId;
            });
    }

    public function save(FollowerProjection $followerProjection)
    {
        $this->projectionStore->store($followerProjection->getFolloweeId()->getId(), $followerProjection);
    }
}