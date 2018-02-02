<?php

namespace Tests\Domain;

use App\Domain\IDomainEvent;
use App\Domain\IEventPublisher;

class FakeEventPublisher implements IEventPublisher {

    /**
     * @var array
     */
    public $events = [];

    public function publish(IDomainEvent $event)
    {
        $this->events[] = $event;
    }
}