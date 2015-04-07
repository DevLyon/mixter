<?php

namespace Infrastructure\Identity;

use App\Domain\Identity\SessionId;
use App\Domain\Identity\UserConnected;
use App\Domain\Identity\UserId;
use App\Infrastructure\Identity\SessionRepository;
use Tests\Infrastructure\InMemoryEventStore;

class SessionRepositoryTest extends \PHPUnit_Framework_TestCase {
    public function testGivenAUserIsConnected_WhenGetBySessionId_ThenReturnsSession() {
        $userConnected = new UserConnected(new UserId('clement@mix-it.fr'), SessionId::generate(), new \DateTime());
        $eventStore = new InMemoryEventStore(array($userConnected));
        $sessionRepository = new SessionRepository($eventStore);

        $session = $sessionRepository->get($userConnected->getSessionId());

        \Assert\that($session)->notNull();
    }
}