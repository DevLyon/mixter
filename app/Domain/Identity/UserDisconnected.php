<?php

namespace App\Domain\Identity;

use App\Domain\IDomainEvent;

class UserDisconnected implements IDomainEvent {
    /**
     * @var UserId
     */
    private $userId;

    /**
     * @var SessionId
     */
    private $sessionId;

    public function __construct(UserId $userId, SessionId $sessionId) {
        $this->userId = $userId;
        $this->sessionId = $sessionId;
    }

    /**
     * @return UserId
     */
    public function getUserId()
    {
        return $this->userId;
    }

    /**
     * @return SessionId
     */
    public function getSessionId()
    {
        return $this->sessionId;
    }

    public function getAggregateId()
    {
        return $this->getSessionId()->getId();
    }
}