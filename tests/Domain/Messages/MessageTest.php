<?php namespace Tests\Domain\Messages;

use App\Domain\Identity\UserId;
use App\Domain\Messages\Message;
use App\Domain\Messages\MessageDeleted;
use App\Domain\Messages\MessageId;
use App\Domain\Messages\MessageQuacked;
use App\Domain\Messages\MessageRequacked;
use Tests\Domain\FakeEventPublisher;

class MessageTest extends \PHPUnit_Framework_TestCase
{
    public function testWhenQuackAMessage_ThenMessageQuackedIsRaised()
    {
        $fakeEventPublisher = new FakeEventPublisher();
        $authorId = new UserId('clem@mix-it.fr');

        Message::quack($fakeEventPublisher, 'hello', $authorId);

        \Assert\that($fakeEventPublisher->events)->count(1);
        /** @var MessageQuacked $messageQuacked */
        $messageQuacked = $fakeEventPublisher->events[0];
        \Assert\that($messageQuacked->getMessageId())->notNull();
        \Assert\that($messageQuacked->getContent())->eq('hello');
        \Assert\that($messageQuacked->getAuthorId())->eq($authorId);
    }

    public function testWhenQuackTwoMessages_ThenTwoDifferentMessageQuackedAreRaised()
    {
        $fakeEventPublisher = new FakeEventPublisher();
        $authorId = new UserId('clem@mix-it.fr');

        Message::quack($fakeEventPublisher, 'hello', $authorId);
        Message::quack($fakeEventPublisher, 'how are you ?', $authorId);

        \Assert\that($fakeEventPublisher->events)->count(2);
        /** @var MessageQuacked $firstMessageQuacked */
        $firstMessageQuacked = $fakeEventPublisher->events[0];
        /** @var MessageQuacked $secondMessageQuacked */
        $secondMessageQuacked = $fakeEventPublisher->events[1];
        \Assert\that($firstMessageQuacked->getMessageId())
            ->notEq($secondMessageQuacked->getMessageId());
    }

    public function testWhenRequackAMessage_ThenMessageRequackedIsRaised()
    {
        $fakeEventPublisher = new FakeEventPublisher();
        $authorId = new UserId('clem@mix-it.fr');
        $messageQuacked = new MessageQuacked(MessageId::generate(), 'Hello', $authorId);
        $message = new Message(array($messageQuacked));
        $requackerId = new UserId('emilien@mix-it.fr');

        $message->requack($fakeEventPublisher, $requackerId);

        \Assert\that($fakeEventPublisher->events)->count(1);
        /** @var MessageRequacked $messageRequacked */
        $messageRequacked = $fakeEventPublisher->events[0];
        \Assert\that($messageRequacked->getMessageId())->eq($messageQuacked->getMessageId());
        \Assert\that($messageRequacked->getRequackerId())->eq($requackerId);
    }

    public function testWhenAuthorRequackItsOwnMessage_ThenNothingHappens()
    {
        $fakeEventPublisher = new FakeEventPublisher();
        $authorId = new UserId('clem@mix-it.fr');
        $messageQuacked = new MessageQuacked(MessageId::generate(), 'Hello', $authorId);
        $message = new Message(array($messageQuacked));

        $message->requack($fakeEventPublisher, $authorId);

        \Assert\that($fakeEventPublisher->events)->count(0);
    }

    public function testWhenRequackerRequackASecondTime_ThenNothingHappens()
    {
        $fakeEventPublisher = new FakeEventPublisher();
        $authorId = new UserId('clem@mix-it.fr');
        $messageQuacked = new MessageQuacked(MessageId::generate(), 'Hello', $authorId);
        $requackerId = new UserId('emilien@mix-it.fr');
        $messageRequacked = new MessageRequacked($messageQuacked->getMessageId(), $requackerId);
        $message = new Message(array($messageQuacked, $messageRequacked));

        $message->requack($fakeEventPublisher, $requackerId);

        \Assert\that($fakeEventPublisher->events)->count(0);
    }

    public function testWhenAuthorDeleteMessage_ThenMessageDeletedIsRaised()
    {
        $fakeEventPublisher = new FakeEventPublisher();
        $authorId = new UserId('clem@mix-it.fr');
        $messageQuacked = new MessageQuacked(MessageId::generate(), 'Hello', $authorId);
        $message = new Message(array($messageQuacked));

        $message->delete($fakeEventPublisher, $authorId);

        \Assert\that($fakeEventPublisher->events)->count(1);
        /** @var MessageDeleted $messageDeleted */
        $messageDeleted = $fakeEventPublisher->events[0];
        \Assert\that($messageDeleted->getMessageId())->eq($messageQuacked->getMessageId());
        \Assert\that($messageDeleted->getDeleterId())->eq($authorId);
    }

    public function testWhenSomeoneElseThanAuthorDeleteMessage_ThenNothingHappens()
    {
        $fakeEventPublisher = new FakeEventPublisher();
        $authorId = new UserId('clem@mix-it.fr');
        $messageQuacked = new MessageQuacked(MessageId::generate(), 'Hello', $authorId);
        $message = new Message(array($messageQuacked));
        $deleterid = new UserId('jean@mixt-it.fr');

        $message->delete($fakeEventPublisher, $deleterid);

        \Assert\that($fakeEventPublisher->events)->count(0);
    }
}