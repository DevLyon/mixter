<?php

namespace App\Domain\Messages {

    use App\Domain\Identity\UserId;
    use App\Domain\IEventPublisher;
    use App\Domain\Messages\Message\DecisionProjection;

    class Message
    {
        public static function quack(IEventPublisher $eventPublisher, $messageContent, UserId $authorId)
        {
            $eventPublisher->publish(new MessageQuacked(MessageId::generate(), $messageContent, $authorId));
        }

        public function __construct($events)
        {
            $this->decisionProjection = new DecisionProjection($events);
        }

        public function requack(IEventPublisher $eventPublisher, UserId $requackerId)
        {
            if($requackerId == $this->decisionProjection->getAuthorId()
                || in_array($requackerId, $this->decisionProjection->getRequackers())) {
                return;
            }
            $eventPublisher->publish(
                new MessageRequacked($this->decisionProjection->getMessageId(), $requackerId));
        }
    }
}

namespace App\Domain\Messages\Message {

    use App\Domain\DecisionProjectionBase;
    use App\Domain\Identity\UserId;
    use App\Domain\Messages\MessageId;
    use App\Domain\Messages\MessageQuacked;
    use App\Domain\Messages\MessageRequacked;

    class DecisionProjection extends DecisionProjectionBase
    {
        /** @var MessageId */
        private $messageId;

        /** @var UserId */
        private $authorId;

        /** @var array */
        private $requackers = array();

        public function __construct($events)
        {
            $this->registerMessageQuacked();
            $this->registerMessageRequacked();
            parent::__construct($events);
        }

        /**
         * @return MessageId
         */
        public function getMessageId()
        {
            return $this->messageId;
        }

        /**
         * @return UserId
         */
        public function getAuthorId()
        {
            return $this->authorId;
        }

        /**
         * @return array
         */
        public function getRequackers()
        {
            return $this->requackers;
        }

        private function registerMessageQuacked()
        {
            $this->register('App\Domain\Messages\MessageQuacked', function (MessageQuacked $event) {
                $this->messageId = $event->getMessageId();
                $this->authorId = $event->getAuthorId();
            });
        }

        private function registerMessageRequacked()
        {
            $this->register('App\Domain\Messages\MessageRequacked', function (MessageRequacked $event) {
                $this->requackers[] = $event->getRequackerId();
            });
        }
    }
}