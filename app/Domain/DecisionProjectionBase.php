<?php

namespace App\Domain;

class DecisionProjectionBase {
    /** @var array */
    private $registeredEvents;

    /** @param array $events */
    public function __construct($events)
    {
        foreach ($events as $event) {
            $this->apply($event);
        }
    }

    protected function register($eventTypeName, $callback) {
        $this->registeredEvents[$eventTypeName] = $callback;
    }

    /**
     * @param IDomainEvent $event
     */
    public function apply(IDomainEvent $event)
    {
        $eventType = get_class($event);
        if(!array_key_exists($eventType, $this->registeredEvents)) {
            return;
        }

        $applyCallback = $this->registeredEvents[$eventType];
        $applyCallback($event);
    }
}