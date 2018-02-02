<?php

namespace Tests\Infrastructure\Timeline;

use App\Domain\Identity\UserId;
use App\Domain\Messages\MessageId;
use App\Domain\Timeline\TimelineMessage;
use App\Infrastructure\Timeline\TimelineMessageRepository;
use Tests\Infrastructure\InMemoryProjectionStore;

class TimelineMessageRepositoryTest extends \PHPUnit_Framework_TestCase
{
    public function testGivenExistingTimelineMessage_WhenGetByMessageId_ThenReturnsTimelineMessage()
    {
        $existingTimelineMessage = new TimelineMessage(MessageId::generate(), 'hello', new UserId('clem@mix-it.fr'));
        $projectionStore = new InMemoryProjectionStore(array($existingTimelineMessage->getMessageId()->getId() => $existingTimelineMessage));
        $timelineMessageRepository = new TimelineMessageRepository($projectionStore);

        $timelineMessage = $timelineMessageRepository->getByMessageId($existingTimelineMessage->getMessageId());

        \Assert\that($timelineMessage)->eq($existingTimelineMessage);
    }

    public function testGivenTimelineMessageDoesNotExist_WhenGetByMessageId_ThenReturnsNull()
    {
        $projectionStore = new InMemoryProjectionStore();
        $timelineMessageRepository = new TimelineMessageRepository($projectionStore);

        $timelineMessage = $timelineMessageRepository->getByMessageId(MessageId::generate());

        \Assert\that(is_null($timelineMessage))->true();
    }

    public function testWhenSaveTimelineMessage_ThenStoresIt()
    {
        $projectionStore = new InMemoryProjectionStore();
        $timelineMessageRepository = new TimelineMessageRepository($projectionStore);

        $timelineMessage = new TimelineMessage(MessageId::generate(), 'hello', new UserId('clem@mix-it.fr'));
        $timelineMessageRepository->save($timelineMessage);

        $storedMessage = $projectionStore->get($timelineMessage->getMessageId()->getId(), get_class($timelineMessage));
        \Assert\that($storedMessage)->eq($timelineMessage);
    }

    public function testGiven3ExistingTimelineMessages_WhenGetAll_ThenReturnThe3TimelineMessages()
    {
        $timelineMessage1 = new TimelineMessage(MessageId::generate(), 'hello', new UserId('clem@mix-it.fr'));
        $timelineMessage2 = new TimelineMessage(MessageId::generate(), 'hello', new UserId('clem@mix-it.fr'));
        $timelineMessage3 = new TimelineMessage(MessageId::generate(), 'hello', new UserId('clem@mix-it.fr'));
        $projectionStore = new InMemoryProjectionStore(
            array(
                $timelineMessage1->getMessageId()->getId() => $timelineMessage1,
                $timelineMessage2->getMessageId()->getId() => $timelineMessage2,
                $timelineMessage3->getMessageId()->getId() => $timelineMessage3));
        $timelineMessageRepository = new TimelineMessageRepository($projectionStore);

        \Assert\that($timelineMessageRepository->getAll())->count(3);
    }

    public function testGiven3ExistingTimelineMessages_WhenGetByOwnerId_ThenReturnOnlyThe2CorrespondingTimelineMessages()
    {
        $ownerId = new UserId('clem@mix-it.fr');
        $timelineMessage1 = new TimelineMessage(MessageId::generate(), 'hello', $ownerId);
        $timelineMessage2 = new TimelineMessage(MessageId::generate(), 'hello', new UserId('jean@mix-it.fr'));
        $timelineMessage3 = new TimelineMessage(MessageId::generate(), 'hello', $ownerId);
        $projectionStore = new InMemoryProjectionStore(
            array(
                $timelineMessage1->getMessageId()->getId() => $timelineMessage1,
                $timelineMessage2->getMessageId()->getId() => $timelineMessage2,
                $timelineMessage3->getMessageId()->getId() => $timelineMessage3));
        $timelineMessageRepository = new TimelineMessageRepository($projectionStore);

        \Assert\that($timelineMessageRepository->getByOwnerId($ownerId))->count(2);
    }
}