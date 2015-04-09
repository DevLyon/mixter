<?php

namespace App\Domain\Subscriptions;

use App\Domain\Identity\UserId;
use App\Domain\IEventPublisher;

class Subscription
{
    public static function followUser(IEventPublisher $eventPublisher, UserId $followerId, UserId $followeeId)
    {
    }
}