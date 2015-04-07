<?php

namespace App\Domain\Messages;

interface IMessageRepository
{
    /**
     * @param MessageId $messageId
     * @return Message
     */
    public function get(MessageId $messageId);
}