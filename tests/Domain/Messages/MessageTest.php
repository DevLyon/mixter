<?php namespace Tests\Domain\Messages;

use App\Domain\Messages\Message;
use App\Domain\Messages\MessagePublished;
use Tests\Domain\FakeEventPublisher;

class MessageTest extends \PHPUnit_Framework_TestCase {
    public function testWhenPublishAMessage_ThenMessagePublishedIsRaised() {
        $fakeEventPublisher = new FakeEventPublisher();

        Message::publish($fakeEventPublisher, 'hello');

        \Assert\that($fakeEventPublisher->events)->count(1);
        /** @var MessagePublished $messagePublished */
        $messagePublished = $fakeEventPublisher->events[0];
        \Assert\that($messagePublished->getMessageId())->notNull();
        \Assert\that($messagePublished->getContent())->eq('hello');
    }
}