<?php

namespace App\Domain\Identity;

use App\Domain\DecisionProjectionBase;
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

class DecisionProjection extends DecisionProjectionBase {

    /** @var string */
    private $userId;

    /**
     * @param array $events
     */
    public function __construct($events) {
        $this->registerUserRegistered();
        parent::__construct($events);
    }

    /**
     * @return string
     */
    public function getUserId()
    {
        return $this->userId;
    }

    private function registerUserRegistered()
    {
        $this->register('App\Domain\Identity\UserRegistered', function(UserRegistered $event) {
            $this->userId = $event->getUserId();
        });
    }
}