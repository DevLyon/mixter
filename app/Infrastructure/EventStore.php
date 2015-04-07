<?php

namespace App\Infrastructure;

use App\Domain\IDomainEvent;
use App\Domain\UnknownAggregate;
use Prophecy\Exception\Prediction\AggregateException;

class EventStore {

    /** @var array */
    private $eventsByAggregate = array();

    /**
     * @param array $events
     */
    public function __construct($events = array()) {
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