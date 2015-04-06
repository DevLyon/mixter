<?php

namespace App\Domain\Identity;

class SessionRepository {
    /** @var array */
    private $sessionProjections;

    /**
     * @param SessionId $sessionId
     * @return UserId
     */
    public function getUserIdOfSessionId(SessionId $sessionId)
    {
        /** @var SessionProjection $sessionProjection */
        $sessionProjection = $this->sessionProjections[$sessionId->getId()];
        if (is_null($sessionProjection)) {
            return null;
        }
        return $sessionProjection->getUserId();
    }

    public function save(SessionProjection $sessionProjection)
    {
        $this->sessionProjections[$sessionProjection->getSessionId()->getId()] = $sessionProjection;
    }
}