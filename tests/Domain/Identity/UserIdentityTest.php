<?php

namespace Tests\Domain\Identity;

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
}