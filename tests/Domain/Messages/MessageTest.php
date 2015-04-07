<?php namespace Tests\Domain\Messages;

use App\Domain\Messages\Message;
use App\Domain\Messages\MessageId;
use App\Domain\Messages\MessagePublished;
use App\Domain\Messages\MessageRepublished;
use Tests\Domain\FakeEventPublisher;

class MessageTest extends \PHPUnit_Framework_TestCase
{
    public function testWhenPublishAMessage_ThenMessagePublishedIsRaised()
    {
        $fakeEventPublisher = new FakeEventPublisher();

        Message::publish($fakeEventPublisher, 'hello');

        \Assert\that($fakeEventPublisher->events)->count(1);
        /** @var MessagePublished $messagePublished */
        $messagePublished = $fakeEventPublisher->events[0];
        \Assert\that($messagePublished->getMessageId())->notNull();
        \Assert\that($messagePublished->getContent())->eq('hello');
    }

    public function testWhenPublishTwoMessages_ThenTwoDifferentMessagePublishedAreRaised()
    {
        $fakeEventPublisher = new FakeEventPublisher();

        Message::publish($fakeEventPublisher, 'hello');
        Message::publish($fakeEventPublisher, 'how are you ?');

        \Assert\that($fakeEventPublisher->events)->count(2);
        /** @var MessagePublished $firstMessagePublished */
        $firstMessagePublished = $fakeEventPublisher->events[0];
        /** @var MessagePublished $secondMessagePublished */
        $secondMessagePublished = $fakeEventPublisher->events[1];
        \Assert\that($firstMessagePublished->getMessageId())
            ->notEq($secondMessagePublished->getMessageId());
    }

    public function testWhenRepublishAMessage_ThenMessageRepublishedIsRaised()
    {
        $fakeEventPublisher = new FakeEventPublisher();
        $messagePublished = new MessagePublished(MessageId::generate(), 'Hello');
        $message = new Message(array($messagePublished));

        $message->republish($fakeEventPublisher);

        \Assert\that($fakeEventPublisher->events)->count(1);
        /** @var MessageRepublished $messageRepublished */
        $messageRepublished = $fakeEventPublisher->events[0];
        \Assert\that($messageRepublished->getMessageId())->eq($messagePublished->getMessageId());
    }
}