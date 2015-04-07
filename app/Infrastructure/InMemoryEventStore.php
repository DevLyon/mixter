<?php

namespace App\Infrastructure;

use App\Domain\IDomainEvent;
use App\Domain\UnknownAggregate;

class InMemoryEventStore implements IEventStore
{

    /** @var array */
    private $eventsByAggregate = array();

    /**
     * @param array $events
     */
    public function __construct($events = array()) {
        /** @var IDomainEvent $event */
        foreach ($events as $event) {
            $this->storeEvent($event);
        }
    }

    /**
     * Called through Laravel Event Listener
     * @param IDomainEvent $event
     */
    public function storeEvent(IDomainEvent $event) {
        $aggregateId = $event->getAggregateId();
        if (empty($this->eventsByAggregate[$aggregateId])) {
            $this->eventsByAggregate[$aggregateId] = array($event);
        } else {
            $this->eventsByAggregate[$aggregateId][] = $event;
        }
    }

    /**
     * @param string $aggregateId
     * @return array
     * @throws UnknownAggregate
     */
    public function getEvents($aggregateId)
    {
        if (array_key_exists($aggregateId, $this->eventsByAggregate)) {
            return $this->eventsByAggregate[$aggregateId];
        }
        throw new UnknownAggregate();
    }
}