<?php

namespace App\Domain\Messages;

use App\Domain\Identity\UserId;
use App\Domain\IDomainEvent;

class MessageDeleted implements IDomainEvent
{
    private $messageId;
    private $authorId;

    public function __construct(MessageId $messageId, UserId $authorId)
    {
        $this->messageId = $messageId;
        $this->authorId = $authorId;
    }

    public function getAggregateId(): string
    {
        return $this->messageId->getId();
    }

    public function getMessageId(): MessageId
    {
        return $this->messageId;
    }

    public function getDeleterId(): UserId
    {
        return $this->authorId;
    }
}
