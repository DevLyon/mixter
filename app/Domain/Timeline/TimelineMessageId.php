<?php

namespace App\Domain\Timeline;

use App\Domain\Identity\UserId;
use App\Domain\Messages\MessageId;

class TimelineMessageId
{
    /**
     * @var MessageId
     */
    private $messageId;

    /**
     * @var UserId
     */
    private $ownerId;

    public function __construct(MessageId $messageId, UserId $ownerId)
    {
        $this->messageId = $messageId;
        $this->ownerId = $ownerId;
    }

    public function getId()
    {
        return $this->messageId->getId().'_'.$this->ownerId->getId();
    }
}