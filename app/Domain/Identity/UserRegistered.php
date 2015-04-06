<?php

namespace App\Domain\Identity;

use App\Domain\IDomainEvent;

class UserRegistered implements IDomainEvent {
    /**
     * @var UserId
     */
    private $userId;

    public function __construct(UserId $userId) {
        $this->userId = $userId;
    }

    /**
     * @return UserId
     */
    public function getUserId()
    {
        return $this->userId;
    }
}