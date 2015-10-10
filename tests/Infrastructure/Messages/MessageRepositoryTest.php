<?php

namespace Tests\Infrastructure\Messages;

use App\Domain\Identity\UserId;
use App\Domain\Messages\MessageId;
use App\Domain\Messages\MessageQuacked;
use App\Infrastructure\Messages\MessageRepository;
use Tests\Infrastructure\InMemoryEventStore;

class MessageRepositoryTest extends \PHPUnit_Framework_TestCase
{
    public function testGivenAMessageHaveBeenQuacked_WhenGetByMessageId_ThenReturnsMessageAggregate()
    {
        $messageQuacked = new MessageQuacked(MessageId::generate(), 'Hello', new UserId('clem@mix-it.fr'));
        $eventStore = new InMemoryEventStore(array($messageQuacked));
        $messageRepository = new MessageRepository($eventStore);

        $message = $messageRepository->get($messageQuacked->getMessageId());

        \Assert\that($message)->notNull();
    }
}