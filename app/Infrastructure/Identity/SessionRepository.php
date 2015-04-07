<?php

namespace App\Infrastructure\Identity;

use App\Domain\Identity\ISessionRepository;
use App\Domain\Identity\SessionId;
use App\Domain\Identity\SessionProjection;
use App\Domain\Identity\UserId;

class SessionRepository implements ISessionRepository
{
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

    public function remove(SessionId $sessionId)
    {
        // TODO: Implement remove() method.
    }
}