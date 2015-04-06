<?php

namespace App\Domain\Identity;

use App\Domain\IDomainEvent;

class UserConnected implements IDomainEvent {
    /**
     * @var string
     */
    private $userId;

    /**
     * @var string
     */
    private $sessionId;

    /**
     * @var \DateTime
     */
    private $connectedAt;

    public function __construct($userId, $sessionId, $connectedAt) {

        $this->userId = $userId;
        $this->sessionId = $sessionId;
        $this->connectedAt = $connectedAt;
    }

    /**
     * @return string
     */
    public function getUserId()
    {
        return $this->userId;
    }

    /**
     * @return string
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
}