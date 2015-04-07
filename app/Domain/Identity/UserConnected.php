<?php

namespace App\Domain\Identity;

use App\Domain\IDomainEvent;

class UserConnected implements IDomainEvent {
    /**
     * @var UserId
     */
    private $userId;

    /**
     * @var SessionId
     */
    private $sessionId;

    /**
     * @var \DateTime
     */
    private $connectedAt;

    public function __construct(UserId $userId, SessionId $sessionId, \DateTime $connectedAt) {
        $this->userId = $userId;
        $this->sessionId = $sessionId;
        $this->connectedAt = $connectedAt;
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

    /**
     * @return \DateTime
     */
    public function getConnectedAt()
    {
        return $this->connectedAt;
    }

    public function getAggregateId()
    {
        return $this->getSessionId()->getId();
    }
}