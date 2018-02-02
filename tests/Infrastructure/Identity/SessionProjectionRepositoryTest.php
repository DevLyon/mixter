<?php

namespace Tests\Domain\Identity;

use App\Domain\Identity\SessionId;
use App\Domain\Identity\SessionProjection;
use App\Domain\Identity\UserId;
use App\Infrastructure\Identity\SessionProjectionRepository;
use Tests\Infrastructure\InMemoryProjectionStore;

class SessionRepositoryTest extends \PHPUnit_Framework_TestCase
{
    public function testGivenNoProjections_WhenGetUserIdOfSessionId_ThenReturnNull()
    {
        $sessionRepository = new SessionProjectionRepository(new InMemoryProjectionStore());

        $userId = $sessionRepository->getUserIdOfSessionId(SessionId::generate());

        \Assert\that(is_null($userId))->true();
    }

    public function testGivenSeveralUsersAreConnected_WhenGetUserIdOfSessionId_ThenReturnUserIdOfThisSession()
    {
        $sessionRepository = new SessionProjectionRepository(new InMemoryProjectionStore());
        $currentSession = new SessionProjection(new UserId('emilien@mix-it.fr'), SessionId::generate());
        $sessionRepository->save($currentSession);
        $sessionRepository->save(new SessionProjection(new UserId('jean@mix-it.fr'), SessionId::generate()));

        $userId = $sessionRepository->getUserIdOfSessionId($currentSession->getSessionId());

        \Assert\that($userId)->eq($currentSession->getUserId());
    }

    public function testGivenSeveralUsersAreConnected_WhenGetAll_ThenReturnAllSessions()
    {
        $sessionRepository = new SessionProjectionRepository(new InMemoryProjectionStore());
        $currentSession = new SessionProjection(new UserId('emilien@mix-it.fr'), SessionId::generate());
        $sessionRepository->save($currentSession);
        $sessionRepository->save(new SessionProjection(new UserId('jean@mix-it.fr'), SessionId::generate()));

        $allSessions = $sessionRepository->getAll();

        \Assert\that($allSessions)->count(2);
    }

    public function testGivenAUserIsConnected_WhenRemove_ThenProjectionIsRemoved() {
        $sessionRepository = new SessionProjectionRepository(new InMemoryProjectionStore());
        $currentSession = new SessionProjection(new UserId('emilien@mix-it.fr'), SessionId::generate());
        $sessionRepository->save($currentSession);

        $sessionRepository->remove($currentSession->getSessionId());

        $userId = $sessionRepository->getUserIdOfSessionId($currentSession->getSessionId());
        \Assert\that(is_null($userId))->true();
    }
}