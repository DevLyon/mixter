<?php

namespace Tests\Domain\Identity;

use App\Domain\Identity\UserConnected;
use App\Domain\Identity\UserIdentity;
use App\Domain\Identity\UserRegistered;
use Tests\Domain\FakeEventPublisher;

class UserIdentityTest extends \PHPUnit_Framework_TestCase {
    public function testWhenRegisterUser_ThenUserRegisteredIsRaised() {
        $fakeEventPublisher = new FakeEventPublisher();
        $userId = 'clem@mix-it.fr';

        UserIdentity::register($fakeEventPublisher, $userId);

        \Assert\that($fakeEventPublisher->events)->count(1);
        /** @var UserRegistered $userRegistered */
        $userRegistered = $fakeEventPublisher->events[0];
        \Assert\that($userRegistered->getUserId())->eq($userId);
    }

    public function testWhenLogIn_ThenUserConnectedIsRaised() {
        $fakeEventPublisher = new FakeEventPublisher();
        $userId = 'clem@mix-it.fr';
        $userIdentity = new UserIdentity(array(new UserRegistered($userId)));

        $userIdentity->logIn($fakeEventPublisher);

        \Assert\that($fakeEventPublisher->events)->count(1);
        /** @var UserConnected $userConnected */
        $userConnected = $fakeEventPublisher->events[0];
        \Assert\that($userConnected)->isInstanceOf('App\Domain\Identity\UserConnected');
        \Assert\that($userConnected->getUserId())->eq($userId);
        \Assert\that($userConnected->getSessionId())->notNull();
        \Assert\that($userConnected->getConnectedAt()->diff(new \DateTime())->s)->range(-1, 1);
    }
}