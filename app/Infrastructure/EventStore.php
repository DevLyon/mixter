<?php

namespace App\Infrastructure;

use App\Domain\IDomainEvent;

class EventStore {

    /** @var array */
    private $eventsByAggregate;

    /**
     * @param array $events
     */
    public function __construct($events) {
        /** @var IDomainEvent $event */
        foreach ($events as $event) {
            if (empty($this->eventsByAggregate[$event->getAggregateId()])) {
                $this->eventsByAggregate[$event->getAggregateId()] = array($event);
            } else {
                $this->eventsByAggregate[$event->getAggregateId()][] = $event;
            }
        }
    }

    /**
     * @param string $aggregateId
     * @return array
     */
    public function getEvents($aggregateId)
    {
        return $this->eventsByAggregate[$aggregateId];
    }
}