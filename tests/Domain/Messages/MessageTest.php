<?php namespace Tests\Domain\Messages;

use App\Domain\Identity\UserId;
use App\Domain\Messages\Message;
use App\Domain\Messages\MessageDeleted;
use App\Domain\Messages\MessageId;
use App\Domain\Messages\MessagePublished;
use App\Domain\Messages\MessageRepublished;
use App\Domain\Messages\ReplyMessagePublished;
use Symfony\Component\Security\Core\User\User;
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

    public function testWhenRepublisherRepublishASecondTime_ThenNothingHappens()
    {
        $fakeEventPublisher = new FakeEventPublisher();
        $authorId = new UserId('clem@mix-it.fr');
        $messagePublished = new MessagePublished(MessageId::generate(), 'Hello', $authorId);
        $republisherId = new UserId('emilien@mix-it.fr');
        $messageRepublished = new MessageRepublished($messagePublished->getMessageId(), $republisherId);
        $message = new Message(array($messagePublished, $messageRepublished));

        $message->republish($fakeEventPublisher, $republisherId);

        \Assert\that($fakeEventPublisher->events)->count(0);
    }

    public function testWhenReplyToAMessage_ThenReplyMessagePublishedIsRaised()
    {
        $fakeEventPublisher = new FakeEventPublisher();
        $authorId = new UserId('clem@mix-it.fr');
        $messagePublished = new MessagePublished(MessageId::generate(), 'Hello', $authorId);
        $message = new Message(array($messagePublished));

        $replier = new UserId('emilien@mix-it.fr');
        $replyContent = 'Hello too';
        $message->reply($fakeEventPublisher, $replyContent, $replier);

        \Assert\that($fakeEventPublisher->events)->count(1);
        /** @var ReplyMessagePublished $replyMessagePublished */
        $replyMessagePublished = $fakeEventPublisher->events[0];
        \Assert\that($replyMessagePublished)->isInstanceOf('App\Domain\Messages\ReplyMessagePublished');
        \Assert\that($replyMessagePublished->getReplierId())->eq($replier);
        \Assert\that($replyMessagePublished->getReplyContent())->eq($replyContent);
        \Assert\that($replyMessagePublished->getParentMessageId())->eq($messagePublished->getMessageId());
        \Assert\that($replyMessagePublished->getReplyId())->notNull()->notEq($messagePublished->getMessageId());
        \Assert\that($replyMessagePublished->getAggregateId())->eq($replyMessagePublished->getReplyId()->getId());
    }

    public function testWhenReplyMessageIsRepublished_ThenMessageRepublishedIdIsReplyId()
    {
        $fakeEventPublisher = new FakeEventPublisher();
        $replierId = new UserId('clem@mix-it.fr');
        $replyMessagePublished = new ReplyMessagePublished(MessageId::generate(), 'Hello too', $replierId, MessageId::generate());
        $message = new Message(array($replyMessagePublished));

        $republisherId = new UserId('jean@mix-it.fr');
        $message->republish($fakeEventPublisher, $republisherId);

        \Assert\that($fakeEventPublisher->events)->count(1);
        /** @var MessageRepublished $messageRepublished */
        $messageRepublished = $fakeEventPublisher->events[0];
        \Assert\that($messageRepublished->getMessageId())->eq($replyMessagePublished->getReplyId());
    }

    public function testWhenReplyMessageIsRepublishedByReplier_ThenNothingHappens()
    {
        $fakeEventPublisher = new FakeEventPublisher();
        $replierId = new UserId('clem@mix-it.fr');
        $replyMessagePublished = new ReplyMessagePublished(MessageId::generate(), 'Hello too', $replierId, MessageId::generate());
        $message = new Message(array($replyMessagePublished));

        $message->republish($fakeEventPublisher, $replierId);

        \Assert\that($fakeEventPublisher->events)->count(0);
    }

    public function testWhenAuthorDeleteMessage_ThenMessageDeletedIsRaised()
    {
        $fakeEventPublisher = new FakeEventPublisher();
        $authorId = new UserId('clem@mix-it.fr');
        $messagePublished = new MessagePublished(MessageId::generate(), 'Hello', $authorId);
        $message = new Message(array($messagePublished));

        $message->delete($fakeEventPublisher, $authorId);

        \Assert\that($fakeEventPublisher->events)->count(1);
        /** @var MessageDeleted $messageRepublished */
        $messageRepublished = $fakeEventPublisher->events[0];
        \Assert\that($messageRepublished->getMessageId())->eq($messagePublished->getMessageId());
        \Assert\that($messageRepublished->getDeleterId())->eq($authorId);
    }

    public function testWhenSomeoneElseThanAuthorDeleteMessage_ThenNothingHappens()
    {
        $fakeEventPublisher = new FakeEventPublisher();
        $authorId = new UserId('clem@mix-it.fr');
        $messagePublished = new MessagePublished(MessageId::generate(), 'Hello', $authorId);
        $message = new Message(array($messagePublished));
        $deleterid = new UserId('jean@mixt-it.fr');

        $message->delete($fakeEventPublisher, $deleterid);

        \Assert\that($fakeEventPublisher->events)->count(0);
    }

    public function testGivenAMessageHaveBeenDeleted_WhenDeleteMessage_ThenNothingHappens()
    {
        $fakeEventPublisher = new FakeEventPublisher();
        $authorId = new UserId('clem@mix-it.fr');
        $messagePublished = new MessagePublished(MessageId::generate(), 'Hello', $authorId);
        $messageDeleted = new MessageDeleted($messagePublished->getMessageId(), $authorId);
        $message = new Message(array($messagePublished, $messageDeleted));

        $message->delete($fakeEventPublisher, $authorId);

        \Assert\that($fakeEventPublisher->events)->count(0);
    }

    public function testGivenAMessageHaveBeenDeleted_WhenReplyToMessage_ThenNothingHappens()
    {
        $fakeEventPublisher = new FakeEventPublisher();
        $authorId = new UserId('clem@mix-it.fr');
        $messagePublished = new MessagePublished(MessageId::generate(), 'Hello', $authorId);
        $messageDeleted = new MessageDeleted($messagePublished->getMessageId(), $authorId);
        $message = new Message(array($messagePublished, $messageDeleted));

        $message->reply($fakeEventPublisher, 'Hello too!', $authorId);

        \Assert\that($fakeEventPublisher->events)->count(0);
    }

    public function testGivenAMessageHaveBeenDeleted_WhenRepublishMessage_ThenNothingHappens()
    {
        $fakeEventPublisher = new FakeEventPublisher();
        $authorId = new UserId('clem@mix-it.fr');
        $messagePublished = new MessagePublished(MessageId::generate(), 'Hello', $authorId);
        $messageDeleted = new MessageDeleted($messagePublished->getMessageId(), $authorId);
        $message = new Message(array($messagePublished, $messageDeleted));
        $republisher = new UserId('florent@mix-it.fr');

        $message->republish($fakeEventPublisher, $republisher);

        \Assert\that($fakeEventPublisher->events)->count(0);
    }
}