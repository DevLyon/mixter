<?php

namespace Tests\Domain\Timeline;

use App\Domain\Identity\UserId;
use App\Domain\Messages\MessageId;
use App\Domain\Messages\MessagePublished;
use App\Domain\Timeline\TimelineMessage;
use App\Domain\Timeline\UpdateTimeline;
use App\Infrastructure\Timeline\TimelineMessageRepository;
use Tests\Infrastructure\InMemoryProjectionStore;

class UpdateTimelineTest extends \PHPUnit_Framework_TestCase
{
    public function testWhenHandleMessagePublished_ThenTimelineMessageIsSaved()
    {
        $projectionStore = new InMemoryProjectionStore();
        $timelineMessageRepository = new TimelineMessageRepository($projectionStore);
        $updateTimeline = new UpdateTimeline($timelineMessageRepository);
        $messagePublished = new MessagePublished(MessageId::generate(), 'hello', new UserId('clem@mix-it.fr'));

        $updateTimeline->handleMessagePublished($messagePublished);

        /** @var TimelineMessage $timelineMessage */
        $timelineMessage = $projectionStore->get($messagePublished->getMessageId()->getId(), 'App\Domain\Timeline\TimelineMessage');
        \Assert\that($timelineMessage->getMessageId())->eq($messagePublished->getMessageId());
        \Assert\that($timelineMessage->getOwnerId())->eq($messagePublished->getAuthorId());
        \Assert\that($timelineMessage->getContent())->eq($messagePublished->getContent());
    }
}