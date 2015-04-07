<?php namespace Tests\Domain\Messages;

use App\Domain\Identity\UserId;
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
        $authorId = new UserId('clem@mix-it.fr');

        Message::publish($fakeEventPublisher, 'hello', $authorId);

        \Assert\that($fakeEventPublisher->events)->count(1);
        /** @var MessagePublished $messagePublished */
        $messagePublished = $fakeEventPublisher->events[0];
        \Assert\that($messagePublished->getMessageId())->notNull();
        \Assert\that($messagePublished->getContent())->eq('hello');
        \Assert\that($messagePublished->getAuthorId())->eq($authorId);
    }

    public function testWhenPublishTwoMessages_ThenTwoDifferentMessagePublishedAreRaised()
    {
        $fakeEventPublisher = new FakeEventPublisher();
        $authorId = new UserId('clem@mix-it.fr');

        Message::publish($fakeEventPublisher, 'hello', $authorId);
        Message::publish($fakeEventPublisher, 'how are you ?', $authorId);

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
        $authorId = new UserId('clem@mix-it.fr');
        $messagePublished = new MessagePublished(MessageId::generate(), 'Hello', $authorId);
        $message = new Message(array($messagePublished));
        $republisherId = new UserId('emilien@mix-it.fr');

        $message->republish($fakeEventPublisher, $republisherId);

        \Assert\that($fakeEventPublisher->events)->count(1);
        /** @var MessageRepublished $messageRepublished */
        $messageRepublished = $fakeEventPublisher->events[0];
        \Assert\that($messageRepublished->getMessageId())->eq($messagePublished->getMessageId());
        \Assert\that($messageRepublished->getRepublisherId())->eq($republisherId);
    }

    public function testWhenAuthorRepublishItsOwnMessage_ThenNothingHappens()
    {
        $fakeEventPublisher = new FakeEventPublisher();
        $authorId = new UserId('clem@mix-it.fr');
        $messagePublished = new MessagePublished(MessageId::generate(), 'Hello', $authorId);
        $message = new Message(array($messagePublished));

        $message->republish($fakeEventPublisher, $authorId);

        \Assert\that($fakeEventPublisher->events)->count(0);
    }
}