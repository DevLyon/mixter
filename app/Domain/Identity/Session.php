<?php

namespace App\Domain\Identity {

    use App\Domain\Identity\Session\DecisionProjection;
    use App\Domain\IEventPublisher;

    class Session
    {
        /** @var DecisionProjection */
        private $decisionProjection;

        public static function logIn(IEventPublisher $eventPublisher, UserId $userId)
        {
            $eventPublisher->publish(new UserConnected($userId, SessionId::generate(), new \DateTime()));
        }

        /**
         * @param array $events
         */
        public function __construct($events)
        {
            $this->decisionProjection = new DecisionProjection($events);
        }

        public function logOut(IEventPublisher $eventPublisher)
        {
            $eventPublisher->publish(
                new UserDisconnected(
                    $this->decisionProjection->getUserId(),
                    $this->decisionProjection->getSessionId()));
        }
    }
}

namespace App\Domain\Identity\Session {

    use App\Domain\DecisionProjectionBase;
    use App\Domain\Identity\SessionId;
    use App\Domain\Identity\UserConnected;
    use App\Domain\Identity\UserId;

    class DecisionProjection extends DecisionProjectionBase
    {
        /** @var UserId */
        private $userId;

        /** @var SessionId */
        private $sessionId;

        /**
         * @param array $events
         */
        public function __construct($events)
        {
            $this->registerUserConnected();
            parent::__construct($events);
        }

        private function registerUserConnected()
        {
            $this->register('App\Domain\Identity\UserConnected', function (UserConnected $event) {
                $this->userId = $event->getUserId();
                $this->sessionId = $event->getSessionId();
            });
        }

        /**
         * @return UserId
         */
        public function getUserId()
        {
            return $this->userId;
        }

        /**
         * @return SessionId
         */
        public function getSessionId()
        {
            return $this->sessionId;
        }
    }
}