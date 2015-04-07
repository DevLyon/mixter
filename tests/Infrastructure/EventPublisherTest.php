<?php

namespace Test\Infrastructure;

use App\Domain\IDomainEvent;
use App\Infrastructure\EventPublisher;
use Illuminate\Events\Dispatcher;

class EventPublisherTest extends \PHPUnit_Framework_TestCase {

    public function testGivenAnEventSubscriber_WhenPublishEvent_ThenSubscriberIsCalled() {
        $eventPublisher = new EventPublisher(new Dispatcher());
        $eventA = new EventA();
        $subscriberCalled = false;
        $eventPublisher->subscribe(get_class($eventA), function() use(&$subscriberCalled) {
            $subscriberCalled = true;
        });

        $eventPublisher->publish($eventA);

        \Assert\that($subscriberCalled)->true();
    }
}

class EventA implements IDomainEvent {

}