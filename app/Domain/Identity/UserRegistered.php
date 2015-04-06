<?php

namespace App\Domain\Identity;

use App\Domain\IDomainEvent;

class UserRegistered implements IDomainEvent {
    /**
     * @var string
     */
    private $userId;

    public function __construct($userId) {
        $this->userId = $userId;
    }

    /**
     * @return string
     */
    public function getUserId()
    {
        return $this->userId;
    }
}