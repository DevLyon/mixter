<?php
namespace App\Infrastructure;

use App\Domain\IDomainEvent;
use App\Domain\UnknownAggregate;

interface IEventStore
{
    /**
     * Called through Laravel Event Listener
     * @param IDomainEvent $event
     */
    public function storeEvent(IDomainEvent $event);

    /**
     * @param string $aggregateId
     * @return array
     * @throws UnknownAggregate if no events found for this aggregate (it does not exist)
     */
    public function getEvents($aggregateId);
}