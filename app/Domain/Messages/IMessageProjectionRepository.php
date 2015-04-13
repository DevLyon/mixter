<?php

namespace App\Domain\Messages;

interface IMessageProjectionRepository
{
    public function getById(MessageId $messageId);
}