<?php
namespace App\Domain\Identity;

interface ISessionRepository
{
    /**
     * @param SessionId $sessionId
     * @return UserId
     */
    public function getUserIdOfSessionId(SessionId $sessionId);

    public function save(SessionProjection $sessionProjection);

    public function remove(SessionId $sessionId);

    public function getAll();
}