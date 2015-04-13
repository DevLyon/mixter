<?php

namespace App\Domain\Subscriptions;

use App\Domain\Identity\UserId;
use App\Infrastructure\Subscriptions\FollowerProjectionRepository;

interface IFollowerProjectionRepository
{
    /**
     * @param UserId $followeeId
     * @return array of FollowerProjection
     */
    public function getFollowersOf(UserId $followeeId);

    public function save(FollowerProjection $followerProjection);

    public function remove(FollowerProjection $followerProjection);
}