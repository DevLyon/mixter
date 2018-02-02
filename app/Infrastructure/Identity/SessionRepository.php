<?php

namespace App\Infrastructure\Identity;

use App\Domain\Identity\ISessionRepository;
use App\Domain\Identity\Session;
use App\Domain\Identity\SessionId;
use App\Infrastructure\IEventStore;

class SessionRepository implements ISessionRepository
{
    /** @var IEventStore */
    private $eventStore;

    public function __construct(IEventStore $eventStore)
    {
        $this->eventStore = $eventStore;
    }

    public function get(SessionId $sessionId)
    {
        $events = $this->eventStore->getEvents($sessionId->getId());
        return new Session($events);
    }
}