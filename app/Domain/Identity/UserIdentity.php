<?php

namespace App\Domain\Identity {

    use App\Domain\Identity\UserIdentity\DecisionProjection;
    use App\Domain\IEventPublisher;

    class UserIdentity
    {
        /** @var DecisionProjection */
        private $decisionProjection;

        public static function register(IEventPublisher $eventPublisher, UserId $userId)
        {
            $eventPublisher->publish(new UserRegistered($userId));
        }

        /**
         * @param array $events
         */
        public function __construct($events)
        {
            $this->decisionProjection = new DecisionProjection($events);
        }

        public function logIn(IEventPublisher $eventPublisher)
        {
            Session::logIn($eventPublisher, $this->decisionProjection->getUserId());
        }
    }
}

namespace App\Domain\Identity\UserIdentity {

    use App\Domain\DecisionProjectionBase;
    use App\Domain\Identity\UserId;
    use App\Domain\Identity\UserRegistered;

    class DecisionProjection extends DecisionProjectionBase
    {
        /** @var UserId */
        private $userId;

        /**
         * @param array $events
         */
        public function __construct($events)
        {
            $this->registerUserRegistered();
            parent::__construct($events);
        }

        /**
         * @return UserId
         */
        public function getUserId()
        {
            return $this->userId;
        }

        private function registerUserRegistered()
        {
            $this->register('App\Domain\Identity\UserRegistered', function (UserRegistered $event) {
                $this->userId = $event->getUserId();
            });
        }
    }
}