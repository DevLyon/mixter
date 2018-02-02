<?php

namespace App\Domain\Messages {

    use App\Domain\Identity\UserId;
    use App\Domain\IEventPublisher;
    use App\Domain\Messages\Message\DecisionProjection;

    class Message
    {
        public static function publish(IEventPublisher $eventPublisher, $messageContent, UserId $authorId)
        {
            $eventPublisher->publish(new MessagePublished(MessageId::generate(), $messageContent, $authorId));
        }

        public function __construct($events)
        {
            $this->decisionProjection = new DecisionProjection($events);
        }

        public function republish(IEventPublisher $eventPublisher, UserId $republisherId)
        {
            if($republisherId == $this->decisionProjection->getAuthorId()
                || in_array($republisherId, $this->decisionProjection->getRepublishers())) {
                return;
            }
            $eventPublisher->publish(
                new MessageRepublished($this->decisionProjection->getMessageId(), $republisherId));
        }
    }
}

namespace App\Domain\Messages\Message {

    use App\Domain\DecisionProjectionBase;
    use App\Domain\Identity\UserId;
    use App\Domain\Messages\MessageId;
    use App\Domain\Messages\MessagePublished;
    use App\Domain\Messages\MessageRepublished;

    class DecisionProjection extends DecisionProjectionBase
    {
        /** @var MessageId */
        private $messageId;

        /** @var UserId */
        private $authorId;

        /** @var array */
        private $republishers = array();

        public function __construct($events)
        {
            $this->registerMessagePublished();
            $this->registerMessageRepublished();
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
        public function getRepublishers()
        {
            return $this->republishers;
        }

        private function registerMessagePublished()
        {
            $this->register('App\Domain\Messages\MessagePublished', function (MessagePublished $event) {
                $this->messageId = $event->getMessageId();
                $this->authorId = $event->getAuthorId();
            });
        }

        private function registerMessageRepublished()
        {
            $this->register('App\Domain\Messages\MessageRepublished', function (MessageRepublished $event) {
                $this->republishers[] = $event->getRepublisherId();
            });
        }
    }
}