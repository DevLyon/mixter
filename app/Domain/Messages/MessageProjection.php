<?php

namespace App\Domain\Messages;

class MessageProjection
{
    /**
     * @var MessageId
     */
    private $messageId;

    /** @var string */
    private $content;

    public function __construct(MessageId $messageId, $content)
    {
        $this->messageId = $messageId;
        $this->content = $content;
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
}