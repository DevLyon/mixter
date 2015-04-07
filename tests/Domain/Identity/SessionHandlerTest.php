<?php

namespace Tests\Domain\Identity;

use App\Domain\Identity\ISessionRepository;
use App\Domain\Identity\SessionHandler;
use App\Domain\Identity\SessionId;
use App\Domain\Identity\SessionProjection;
use App\Domain\Identity\UserConnected;
use App\Domain\Identity\UserDisconnected;
use App\Domain\Identity\UserId;

class SessionHandlerTest extends \PHPUnit_Framework_TestCase {
    public function testWhenHandleUserConnected_ThenSessionProjectionIsSaved() {
        $fakeSessionRepository = new FakeSessionRepository();
        $sessionHandler = new SessionHandler($fakeSessionRepository);
        $userConnected = new UserConnected(new UserId("clem@mix-it.fr"), SessionId::generate(), new \DateTime());

        $sessionHandler->handleUserConnected($userConnected);

        \Assert\that($fakeSessionRepository->savedSessionProjections)->count(1);
        /** @var SessionProjection $savedSessionProjection */
        $savedSessionProjection = $fakeSessionRepository->savedSessionProjections[$userConnected->getSessionId()->getId()];
        \Assert\that($savedSessionProjection->getUserId())->eq($userConnected->getUserId());
        \Assert\that($savedSessionProjection->getSessionId())->eq($userConnected->getSessionId());
    }

    public function testWhenHandleUserDisconnected_ThenSessionProjectionIsRemoved() {
        $fakeSessionRepository = new FakeSessionRepository();
        $sessionProjection = new SessionProjection(new UserId("clem@mix-it.fr"), SessionId::generate());
        $fakeSessionRepository->save($sessionProjection);
        $sessionHandler = new SessionHandler($fakeSessionRepository);
        $userDisconnected = new UserDisconnected($sessionProjection->getUserId(), $sessionProjection->getSessionId());

        $sessionHandler->handleUserDisconnected($userDisconnected);

        \Assert\that($fakeSessionRepository->savedSessionProjections)->count(0);
    }
}

class FakeSessionRepository implements ISessionRepository {

    /** @var array */
    public $savedSessionProjections;

    /**
     * @param SessionId $sessionId
     * @return UserId
     */
    public function getUserIdOfSessionId(SessionId $sessionId)
    {
        // TODO: Implement getUserIdOfSessionId() method.
    }

    public function save(SessionProjection $sessionProjection)
    {
        $this->savedSessionProjections[$sessionProjection->getSessionId()->getId()] = $sessionProjection;
    }

    public function remove(SessionId $sessionId)
    {
        unset($this->savedSessionProjections[$sessionId->getId()]);
    }
}