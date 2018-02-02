<?php

namespace App\Domain\Timeline;

use App\Domain\Identity\UserId;
use App\Domain\Messages\MessageId;

class TimelineMessage implements \JsonSerializable
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

    /**
     * (PHP 5 &gt;= 5.4.0)<br/>
     * Specify data which should be serialized to JSON
     * @link http://php.net/manual/en/jsonserializable.jsonserialize.php
     * @return mixed data which can be serialized by <b>json_encode</b>,
     * which is a value of any type other than a resource.
     */
    function jsonSerialize()
    {
        return [
            'messageId' => $this->getMessageId()->getId(),
            'content' => $this->getContent(),
            'ownerId' => $this->getOwnerId()->getId(),
        ];
    }
}