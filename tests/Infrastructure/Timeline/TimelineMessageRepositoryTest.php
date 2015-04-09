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
}