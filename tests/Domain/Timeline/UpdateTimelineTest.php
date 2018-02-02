<?php

namespace Tests\Domain\Timeline;

use App\Domain\Identity\UserId;
use App\Domain\Messages\MessageId;
use App\Domain\Messages\MessageQuacked;
use App\Domain\Timeline\TimelineMessage;
use App\Domain\Timeline\UpdateTimeline;
use App\Infrastructure\Timeline\TimelineMessageRepository;
use Tests\Infrastructure\InMemoryProjectionStore;

class UpdateTimelineTest extends \PHPUnit_Framework_TestCase
{
    public function testWhenHandleMessageQuacked_ThenTimelineMessageIsSaved()
    {
        $projectionStore = new InMemoryProjectionStore();
        $timelineMessageRepository = new TimelineMessageRepository($projectionStore);
        $updateTimeline = new UpdateTimeline($timelineMessageRepository);
        $messageQuacked = new MessageQuacked(MessageId::generate(), 'hello', new UserId('clem@mix-it.fr'));

        $updateTimeline->handleMessageQuacked($messageQuacked);

        /** @var TimelineMessage $timelineMessage */
        $timelineMessage = $projectionStore->get($messageQuacked->getMessageId()->getId(), 'App\Domain\Timeline\TimelineMessage');
        \Assert\that($timelineMessage->getMessageId())->eq($messageQuacked->getMessageId());
        \Assert\that($timelineMessage->getOwnerId())->eq($messageQuacked->getAuthorId());
        \Assert\that($timelineMessage->getContent())->eq($messageQuacked->getContent());
    }
}