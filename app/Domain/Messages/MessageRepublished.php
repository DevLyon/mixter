<?php

namespace App\Domain\Messages;

use App\Domain\IDomainEvent;

class MessageRepublished implements IDomainEvent
{
    /** @var MessageId */
    private $messageId;

    public function __construct(MessageId $messageId)
    {
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
    public function getAggregateId()
    {
        return $this->getMessageId()->getId();
    }
}