<?php

namespace App\Infrastructure\Identity;


use App\Domain\Identity\UserId;
use App\Domain\Identity\UserIdentity;
use App\Infrastructure\EventStore;

class UserIdentityRepository {

    /**
     * @var EventStore
     */
    private $eventStore;

    public function __construct(EventStore $eventStore) {

        $this->eventStore = $eventStore;
    }

    /**
     * @param UserId $userId
     * @return UserIdentity
     */
    public function get(UserId $userId)
    {
        $events = $this->eventStore->getEvents($userId->getId());
        return new UserIdentity($events);
    }
}