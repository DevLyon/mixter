<?php

namespace Tests\Infrastructure;

use App\Domain\IDomainEvent;
use App\Infrastructure\FileEventStore;
use App\Infrastructure\IEventStore;
use Illuminate\Filesystem\Filesystem;
use Illuminate\Filesystem\FilesystemAdapter;
use League\Flysystem\Adapter\Local;

class FileEventStoreTest extends \PHPUnit_Framework_TestCase {

    /** @var string */
    private $aggregateId;

    /** @var Filesystem */
    private $filesystem;

    /** @var IDomainEvent */
    private $event;

    /** @var IEventStore */
    private $eventStore;

    public function setUp() {
        $this->filesystem = new FilesystemAdapter(new \League\Flysystem\Filesystem(new Local(__DIR__.'/../../storage/tests')));
        $this->event = new EventA();
        $this->aggregateId = $this->event->getAggregateId();
        $this->filesystem->delete($this->filesystem->allFiles());
        $this->eventStore = new FileEventStore($this->filesystem);
    }

    public function testGivenAggregateDoesNotExists_WhenStoreEvent_ThenFileIsCreatedWithEventOnOneLine() {
        $this->eventStore->storeEvent($this->event);

        \Assert\that($this->filesystem->exists($this->getPath()))->true();
        $contentLines = explode("\n", $this->filesystem->get($this->getPath()));
        \Assert\that($contentLines)->count(1);
    }

    public function testGivenAggregateExistsWithOneEvent_WhenStoreEvent_ThenAppendEventToExistingFile() {
        $this->eventStore->storeEvent(new EventA());

        $this->eventStore->storeEvent($this->event);

        \Assert\that($this->filesystem->exists($this->getPath()))->true();
        $contentLines = explode("\n", $this->filesystem->get($this->getPath()));
        \Assert\that($contentLines)->count(2);
    }

    public function testGivenAggregateExistsWithTwoEvents_WhenGetEvents_ThenReturnsEventsPreservingOrder() {
        $this->eventStore->storeEvent(new EventA());
        $this->eventStore->storeEvent(new EventB());

        $events = $this->eventStore->getEvents($this->aggregateId);

        \Assert\that($events)->count(2);
        \Assert\that($events[0])->isInstanceOf('Tests\Infrastructure\EventA');
        \Assert\that($events[1])->isInstanceOf('Tests\Infrastructure\EventB');
    }

    public function testGivenAggregateDoesNotExist_WhenGetEvents_ThenThrowUnknownAggregate() {
        $this->setExpectedException('App\Domain\UnknownAggregate');

        $this->eventStore->getEvents($this->aggregateId);
    }

    private function getPath()
    {
        return 'eventStore' . DIRECTORY_SEPARATOR . $this->aggregateId;
    }
}

class EventA implements IDomainEvent {

    /**
     * @return string
     */
    public function getAggregateId()
    {
        return 'fileEventStore';
    }
}

class EventB implements IDomainEvent {

    /**
     * @return string
     */
    public function getAggregateId()
    {
        return 'fileEventStore';
    }
}