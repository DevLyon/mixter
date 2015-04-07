<?php

namespace App\Domain\Identity;

interface ISessionRepository
{
    /**
     * @param SessionId $sessionId
     * @return Session
     */
    public function get(SessionId $sessionId);
}