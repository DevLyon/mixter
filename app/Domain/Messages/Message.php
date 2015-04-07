<?php

namespace App\Domain\Messages {

    use App\Domain\IEventPublisher;
    use App\Domain\Messages\Message\DecisionProjection;

    class Message
    {
        public static function publish(IEventPublisher $eventPublisher, $messageContent)
        {
            $eventPublisher->publish(new MessagePublished(MessageId::generate(), $messageContent));
        }

        public function __construct($events)
        {
            $this->decisionProjection = new DecisionProjection($events);
        }

        public function republish(IEventPublisher $eventPublisher)
        {
            $eventPublisher->publish(new MessageRepublished($this->decisionProjection->getMessageId()));
        }
    }
}

namespace App\Domain\Messages\Message {

    use App\Domain\DecisionProjectionBase;
    use App\Domain\Messages\MessageId;
    use App\Domain\Messages\MessagePublished;

    class DecisionProjection extends DecisionProjectionBase
    {
        /** @var MessageId */
        private $messageId;

        public function __construct($events)
        {
            $this->registerMessagePublished();
            parent::__construct($events);
        }

        /**
         * @return MessageId
         */
        public function getMessageId()
        {
            return $this->messageId;
        }

        private function registerMessagePublished()
        {
            $this->register('App\Domain\Messages\MessagePublished', function (MessagePublished $event) {
                $this->messageId = $event->getMessageId();
            });
        }
    }
}