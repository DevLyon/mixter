<?php

namespace App\Domain\Messages;

use App\Domain\Identity\UserId;
use App\Domain\IDomainEvent;

class MessageDeleted implements IDomainEvent
{
    /**
     * @var MessageId
     */
    private $messageId;

    /**
     * @var UserId
     */
    private $deleterId;

    public function __construct(MessageId $messageId, UserId $deleterId)
    {
        $this->messageId = $messageId;
        $this->deleterId = $deleterId;
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
    public function getDeleterId()
    {
        return $this->deleterId;
    }

    /**
     * @return string
     */
    public function getAggregateId()
    {
        return $this->getMessageId()->getId();
    }
}