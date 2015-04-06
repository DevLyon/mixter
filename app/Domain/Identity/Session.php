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
            if($this->decisionProjection->isDisconnected()) {
                return;
            }

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
    use App\Domain\Identity\UserDisconnected;
    use App\Domain\Identity\UserId;

    class DecisionProjection extends DecisionProjectionBase
    {
        /** @var UserId */
        private $userId;

        /** @var SessionId */
        private $sessionId;

        /** @var bool */
        private $disconnected;

        /**
         * @param array $events
         */
        public function __construct($events)
        {
            $this->registerUserConnected();
            $this->registerUserDisconnected();
            parent::__construct($events);
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

        /**
         * @return boolean
         */
        public function isDisconnected()
        {
            return $this->disconnected;
        }

        private function registerUserConnected()
        {
            $this->register('App\Domain\Identity\UserConnected', function (UserConnected $event) {
                $this->userId = $event->getUserId();
                $this->sessionId = $event->getSessionId();
                $this->disconnected = false;
            });
        }

        private function registerUserDisconnected()
        {
            $this->register('App\Domain\Identity\UserDisconnected', function (UserDisconnected $event) {
                $this->disconnected = true;
            });
        }
    }
}