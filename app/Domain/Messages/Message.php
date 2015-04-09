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

        public function reply(IEventPublisher $eventPublisher, $replyContent, UserId $replier)
        {
            if($this->decisionProjection->isDeleted()) {
                return;
            }
            $replyId = MessageId::generate();
            $eventPublisher->publish(
                new ReplyMessagePublished($replyId, $replyContent, $replier, $this->decisionProjection->getMessageId()));
        }

        public function delete(IEventPublisher $eventPublisher, UserId $deleterId)
        {
            if($this->decisionProjection->isDeleted()
                || $deleterId != $this->decisionProjection->getAuthorId()) {
                return;
            }
            $eventPublisher->publish(
                new MessageDeleted($this->decisionProjection->getMessageId(), $deleterId));
        }
    }
}

namespace App\Domain\Messages\Message {

    use App\Domain\DecisionProjectionBase;
    use App\Domain\Identity\UserId;
    use App\Domain\Messages\MessageDeleted;
    use App\Domain\Messages\MessageId;
    use App\Domain\Messages\MessagePublished;
    use App\Domain\Messages\MessageRepublished;
    use App\Domain\Messages\ReplyMessagePublished;

    class DecisionProjection extends DecisionProjectionBase
    {
        /** @var MessageId */
        private $messageId;

        /** @var UserId */
        private $authorId;

        /** @var array */
        private $republishers = array();

        /** @var bool */
        private $deleted = false;

        public function __construct($events)
        {
            $this->registerMessagePublished();
            $this->registerMessageRepublished();
            $this->registerReplyMessagePublished();
            $this->registerMessageDeleted();
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

        /**
         * @return boolean
         */
        public function isDeleted()
        {
            return $this->deleted;
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

        private function registerReplyMessagePublished()
        {
            $this->register('App\Domain\Messages\ReplyMessagePublished', function (ReplyMessagePublished $event) {
                $this->messageId = $event->getReplyId();
                $this->authorId = $event->getReplierId();
            });
        }

        private function registerMessageDeleted()
        {
            $this->register('App\Domain\Messages\MessageDeleted', function (MessageDeleted $event){
                $this->deleted = true;
            });
        }
    }
}