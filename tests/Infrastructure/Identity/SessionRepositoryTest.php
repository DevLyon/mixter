<?php

namespace Tests\Domain\Identity;

use App\Domain\Identity\SessionId;
use App\Domain\Identity\SessionProjection;
use App\Domain\Identity\UserId;
use App\Infrastructure\Identity\SessionRepository;

class SessionRepositoryTest extends \PHPUnit_Framework_TestCase {
    public function testGivenNoProjections_WhenGetUserIdOfSessionId_ThenReturnNull() {
        $sessionRepository = new SessionRepository();

        $userId = $sessionRepository->getUserIdOfSessionId(SessionId::generate());

        \Assert\that($userId)->nullOr();
    }

    public function testGivenSeveralUsersAreConnected_WhenGetUserIdOfSessionId_ThenReturnUserIdOfThisSession() {
        $sessionRepository = new SessionRepository();
        $currentSession = new SessionProjection(new UserId('emilien@mix-it.fr'), SessionId::generate());
        $sessionRepository->save($currentSession);
        $sessionRepository->save(new SessionProjection(new UserId('jean@mix-it.fr'), SessionId::generate()));

        $userId = $sessionRepository->getUserIdOfSessionId($currentSession->getSessionId());

        \Assert\that($userId)->eq($currentSession->getUserId());
    }
}