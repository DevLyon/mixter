<?php

namespace App\Domain\Subscriptions;

use App\Domain\Identity\UserId;

class SubscriptionId
{
    /**
     * @var UserId
     */
    private $followerId;

    /**
     * @var UserId
     */
    private $followeeId;

    public function __construct(UserId $followerId, UserId $followeeId)
    {

        $this->followerId = $followerId;
        $this->followeeId = $followeeId;
    }

    /**
     * @return UserId
     */
    public function getFollowerId()
    {
        return $this->followerId;
    }

    /**
     * @return UserId
     */
    public function getFolloweeId()
    {
        return $this->followeeId;
    }
}