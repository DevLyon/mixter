<?php

namespace App\Domain\Messages;

use App\Domain\Identity\UserId;
use App\Domain\IDomainEvent;

class MessageQuacked implements IDomainEvent {

    /**
     * @var MessageId
     */
    private $messageId;

    /**
     * @var string
     */
    private $content;

    /**
     * @var UserId
     */
    private $authorId;

    /**
     * @param MessageId $messageId
     * @param string $content
     * @param UserId $authorId
     */
    public function __construct(MessageId $messageId, $content, UserId $authorId) {
        $this->content = $content;
        $this->messageId = $messageId;
        $this->authorId = $authorId;
    }

    /**
     * @return MessageId
     */
    public function getMessageId()
    {
        return $this->messageId;
    }

    /**
     * @return string
     */
    public function getContent()
    {
        return $this->content;
    }

    /**
     * @return UserId
     */
    public function getAuthorId()
    {
        return $this->authorId;
    }

    public function getAggregateId()
    {
        return $this->getMessageId()->getId();
    }
}