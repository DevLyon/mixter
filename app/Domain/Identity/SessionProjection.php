<?php

namespace App\Domain\Identity;

class SessionProjection {
    /**
     * @var SessionId
     */
    private $sessionId;

    /**
     * @var UserId
     */
    private $userId;

    public function __construct(UserId $userId, SessionId $sessionId) {
        $this->sessionId = $sessionId;
        $this->userId = $userId;
    }

    /**
     * @return SessionId
     */
    public function getSessionId()
    {
        return $this->sessionId;
    }

    /**
     * @return UserId
     */
    public function getUserId()
    {
        return $this->userId;
    }
}