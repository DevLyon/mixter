<?php

namespace App\Infrastructure;

use App\Domain\IDomainEvent;
use App\Domain\IEventPublisher;
use Illuminate\Events\Dispatcher;

/**
 * IEventPublisher implementation through Laravel Events
 * @package App\Infrastructure
 */
class EventPublisher implements IEventPublisher {
    /**
     * @var Dispatcher
     */
    private $dispatcher;

    public function __construct(Dispatcher $dispatcher) {
        $this->dispatcher = $dispatcher;
    }

    public function publish(IDomainEvent $event)
    {
        $this->dispatcher->fire($event);
    }

    public function subscribe($eventType, $handler)
    {
        $this->dispatcher->listen($eventType, $handler);
    }
}