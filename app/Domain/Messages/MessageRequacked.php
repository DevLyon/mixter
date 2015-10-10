<?php

namespace App\Domain\Messages;

use App\Domain\Identity\UserId;
use App\Domain\IDomainEvent;

class MessageRequacked implements IDomainEvent
{
    /** @var MessageId */
    private $messageId;

    /** @var UserId */
    private $requackerId;

    public function __construct(MessageId $messageId, UserId $requackerId)
    {
        $this->messageId = $messageId;
        $this->requackerId = $requackerId;
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
    public function getRequackerId()
    {
        return $this->requackerId;
    }

    /**
     * @return string
     */
    public function getAggregateId()
    {
        return $this->getMessageId()->getId();
    }
}