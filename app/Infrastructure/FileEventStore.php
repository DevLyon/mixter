<?php

namespace App\Infrastructure;

use App\Domain\IDomainEvent;
use App\Domain\UnknownAggregate;
use Illuminate\Contracts\Filesystem\Filesystem;

class FileEventStore implements IEventStore {

    /**
     * @var Filesystem
     */
    private $filesystem;

    public function __construct(Filesystem $filesystem) {
        $this->filesystem = $filesystem;
    }

    /**
     * Called through Laravel Event Listener
     * @param IDomainEvent $event
     */
    public function storeEvent(IDomainEvent $event)
    {
        $path = $event->getAggregateId();
        if ($this->filesystem->exists($path)) {
            $this->filesystem->append($path, serialize($event));
        } else {
            $this->filesystem->put($path, serialize($event));
        }
    }

    /**
     * @param string $aggregateId
     * @return array
     * @throws UnknownAggregate
     */
    public function getEvents($aggregateId)
    {
        if ($this->filesystem->exists($aggregateId)) {
            $lines = explode("\n", $this->filesystem->get($aggregateId));
            $events = array();
            foreach ($lines as $line) {
                $events[] = unserialize($line);
            }
            return $events;
        }
        throw new UnknownAggregate();
    }
}