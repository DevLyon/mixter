<?php

namespace App\Domain\Messages;

use App\Domain\Identity\UserId;
use App\Domain\IDomainEvent;

class MessageRepublished implements IDomainEvent
{
    /** @var MessageId */
    private $messageId;

    /** @var UserId */
    private $republisherId;

    public function __construct(MessageId $messageId, UserId $republisherId)
    {
        $this->messageId = $messageId;
        $this->republisherId = $republisherId;
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
    public function getRepublisherId()
    {
        return $this->republisherId;
    }

    /**
     * @return string
     */
    public function getAggregateId()
    {
        return $this->getMessageId()->getId();
    }
}