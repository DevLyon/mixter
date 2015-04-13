<?php

namespace App\Infrastructure\Subscriptions;

use App\Domain\Identity\UserId;
use App\Domain\Subscriptions\FollowerProjection;
use App\Domain\Subscriptions\IFollowerProjectionRepository;
use App\Infrastructure\IProjectionStore;

class FollowerProjectionRepository implements IFollowerProjectionRepository
{
    const PROJECTION_TYPE = 'App\Domain\Subscriptions\FollowerProjection';
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
            $this->projectionStore->getAll(self::PROJECTION_TYPE),
            function(FollowerProjection $item) use($followeeId) {
                return $item->getFolloweeId() == $followeeId;
            });
    }

    public function save(FollowerProjection $followerProjection)
    {
        $this->projectionStore->store($followerProjection->getSubscriptionId()->getId(), $followerProjection);
    }

    public function remove(FollowerProjection $followerProjection)
    {
        $this->projectionStore->remove($followerProjection->getSubscriptionId()->getId(), self::PROJECTION_TYPE);
    }
}