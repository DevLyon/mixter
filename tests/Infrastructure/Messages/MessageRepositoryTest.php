<?php

namespace Tests\Infrastructure\Messages;

use App\Domain\Identity\UserId;
use App\Domain\Messages\MessageId;
use App\Domain\Messages\MessagePublished;
use App\Infrastructure\Messages\MessageRepository;
use Tests\Infrastructure\InMemoryEventStore;

class MessageRepositoryTest extends \PHPUnit_Framework_TestCase
{
    public function testGivenAMessageHaveBeenPublished_WhenGetByMessageId_ThenReturnsMessageAggregate()
    {
        $messagePublished = new MessagePublished(MessageId::generate(), 'Hello', new UserId('clem@mix-it.fr'));
        $eventStore = new InMemoryEventStore(array($messagePublished));
        $messageRepository = new MessageRepository($eventStore);

        $message = $messageRepository->get($messagePublished->getMessageId());

        \Assert\that($message)->notNull();
    }
}