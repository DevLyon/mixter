<?php

namespace App\Domain\Messages;

use App\Domain\IDomainEvent;

class MessagePublished implements IDomainEvent {

    /**
     * @var MessageId
     */
    private $messageId;

    /**
     * @var string
     */
    private $content;

    /**
     * @param MessageId $messageId
     * @param string $content
     */
    public function __construct(MessageId $messageId, $content) {
        $this->content = $content;
        $this->messageId = $messageId;
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

    public function getAggregateId()
    {
        return $this->getMessageId()->getId();
    }
}