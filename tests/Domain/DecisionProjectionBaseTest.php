<?php

namespace Tests\Domain;

use App\Domain\DecisionProjectionBase;
use App\Domain\IDomainEvent;

class DecisionProjectionBaseTest extends \PHPUnit_Framework_TestCase {
    public function testGivenEventAIsRegistered_WhenEventAIsApplied_ThenRegisteredCallbackIsCalled() {
        $decisionProjection = new DecisionProjectionWithEventARegistered(array());
        $eventA = new EventA();

        $decisionProjection->apply($eventA);

        \Assert\that($decisionProjection->eventACallbackCalled)->true();
    }

    public function testGivenEventTypesAreRegistered_WhenSomeEventsAreGivenInConstructorParameters_ThenRegisteredCallbackIsCalled() {
        $eventA = new EventA();
        $eventB = new EventB();

        // It is also called "replay" in Event Sourcing, done through constructor to enforce
        $decisionProjection = new DecisionProjectionWithEventARegistered(array($eventA, $eventB));

        \Assert\that($decisionProjection->eventACallbackCalled)->true();
        \Assert\that($decisionProjection->eventBCallbackCalled)->true();
    }

    public function testGivenEventCIsNotRegistered_WhenEventCIsApplied_ThenNothingHappens() {
        $decisionProjection = new DecisionProjectionWithEventARegistered(array());
        $eventC = new EventC();

        $decisionProjection->apply($eventC);

        \Assert\that($decisionProjection->eventACallbackCalled)->false();
        \Assert\that($decisionProjection->eventBCallbackCalled)->false();
    }
}

class EventA implements IDomainEvent {

    public function getAggregateId()
    {
        return 0;
    }
}

class EventB implements IDomainEvent {

    public function getAggregateId()
    {
        return 0;
    }
}

class EventC implements IDomainEvent {

    public function getAggregateId()
    {
        return 0;
    }
}

class DecisionProjectionWithEventARegistered extends DecisionProjectionBase {
    /** @var bool */
    public $eventACallbackCalled = false;

    /** @var bool */
    public $eventBCallbackCalled = false;

    /**
     * @param array $events
     */
    public function __construct($events) {
        $this->registerEventA();
        $this->registerEventB();
        parent::__construct($events);
    }

    private function registerEventA() {
        $this->register(get_class(new EventA), function(EventA $event) {
            $this->eventACallbackCalled = true;
        });
    }

    private function registerEventB() {
        $this->register(get_class(new EventB), function(EventB $event) {
            $this->eventBCallbackCalled = true;
        });
    }
}