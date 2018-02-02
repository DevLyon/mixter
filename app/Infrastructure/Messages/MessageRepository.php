<?php

namespace App\Infrastructure\Messages;

use App\Domain\Messages\IMessageRepository;
use App\Domain\Messages\Message;
use App\Domain\Messages\MessageId;
use App\Infrastructure\IEventStore;

class MessageRepository implements IMessageRepository
{
    /**
     * @var IEventStore
     */
    private $eventStore;

    public function __construct(IEventStore $eventStore)
    {
        $this->eventStore = $eventStore;
    }

    /**
     * @param MessageId $messageId
     * @return Message
     */
    public function get(MessageId $messageId)
    {
        return new Message($this->eventStore->getEvents($messageId->getId()));
    }
}