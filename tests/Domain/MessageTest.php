<?php namespace Tests\Domain;

use App\Domain\Message;
use App\Domain\MessagePublished;

class MessageTest extends \PHPUnit_Framework_TestCase {
    public function testWhenPublishAMessage_ThenMessagePublishedIsRaised() {
        $fakeEventPublisher = new FakeEventPublisher();

        Message::publish($fakeEventPublisher, 'hello');

        \Assert\that($fakeEventPublisher->events)->count(1);
        /** @var MessagePublished $messagePublished */
        $messagePublished = $fakeEventPublisher->events[0];
        \Assert\that($messagePublished->getContent())->eq('hello');
    }
}