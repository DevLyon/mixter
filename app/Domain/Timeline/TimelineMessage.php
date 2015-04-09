<?php

namespace App\Domain\Timeline;

use App\Domain\Identity\UserId;
use App\Domain\Messages\MessageId;

class TimelineMessage
{
    /**
     * @var MessageId
     */
    private $messageId;

    /** @var string */
    private $content;

    /**
     * @var UserId
     */
    private $ownerId;

    public function __construct(MessageId $messageId, $content, UserId $ownerId)
    {
        $this->messageId = $messageId;
        $this->content = $content;
        $this->ownerId = $ownerId;
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
    public function getOwnerId()
    {
        return $this->ownerId;
    }
}