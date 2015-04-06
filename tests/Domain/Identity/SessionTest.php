<?php

namespace Tests\Domain\Identity;

use App\Domain\Identity\Session;
use App\Domain\Identity\SessionId;
use App\Domain\Identity\UserConnected;
use App\Domain\Identity\UserDisconnected;
use App\Domain\Identity\UserId;
use Tests\Domain\FakeEventPublisher;

class SessionTest extends \PHPUnit_Framework_TestCase {
    public function testWhenLogout_ThenUserDisconnectedEventIsRaised() {
        $fakeEventPublisher = new FakeEventPublisher();
        $userConnected = new UserConnected(
            new UserId("florent@mix-it.fr"),
            SessionId::generate(),
            new \DateTime());
        $session = new Session(array($userConnected));

        $session->logOut($fakeEventPublisher);

        \Assert\that($fakeEventPublisher->events)->count(1);
        /** @var UserDisconnected $userDisconnected */
        $userDisconnected = $fakeEventPublisher->events[0];
        \Assert\that($userDisconnected)->isInstanceOf('App\Domain\Identity\UserDisconnected');
        \Assert\that($userDisconnected->getUserId())->eq($userConnected->getUserId());
        \Assert\that($userDisconnected->getSessionId())->eq($userConnected->getSessionId());
    }
}