<?php

namespace App\Domain\Identity;

use App\Domain\IEventPublisher;

class UserIdentity {
    public static function register(IEventPublisher $eventPublisher, $userId) {
        $eventPublisher->publish(new UserRegistered($userId));
    }
}