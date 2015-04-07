<?php

namespace App\Infrastructure\Identity;


use App\Domain\Identity\UserId;
use App\Domain\Identity\UserIdentity;
use App\Infrastructure\IEventStore;

class UserIdentityRepository {

    /**
     * @var IEventStore
     */
    private $eventStore;

    public function __construct(IEventStore $eventStore) {

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