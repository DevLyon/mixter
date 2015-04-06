<?php

namespace App\Domain\Identity;

use App\Domain\IEventPublisher;

class UserIdentity {
    /** @var DecisionProjection */
    private $decisionProjection;

    public static function register(IEventPublisher $eventPublisher, $userId) {
        $eventPublisher->publish(new UserRegistered($userId));
    }

    /**
     * @param array $events
     */
    public function __construct($events) {
        $this->decisionProjection = new DecisionProjection($events);
    }

    public function logIn(IEventPublisher $eventPublisher) {
        $eventPublisher->publish(new UserConnected($this->decisionProjection->getUserId(), uniqid(), new \DateTime()));
    }
}

class DecisionProjection {

    /** @var string */
    private $userId;

    /**
     * @param array $events
     */
    public function __construct($events) {
        foreach ($events as $event) {
            if (get_class($event) == 'App\Domain\Identity\UserRegistered') {
                /** @var UserRegistered $event */
                $this->userId = $event->getUserId();
            }
        }
    }

    /**
     * @return string
     */
    public function getUserId()
    {
        return $this->userId;
    }
}