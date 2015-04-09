<?php

namespace Tests\Domain\Timeline;

use App\Domain\Identity\UserId;
use App\Domain\Messages\MessageId;
use App\Domain\Messages\MessagePublished;
use App\Domain\Messages\ReplyMessagePublished;
use App\Domain\Timeline\ITimelineMessageRepository;
use App\Domain\Timeline\TimelineMessage;
use App\Domain\Timeline\UpdateTimeline;
use App\Infrastructure\IProjectionStore;
use App\Infrastructure\Timeline\TimelineMessageRepository;
use Tests\Infrastructure\InMemoryProjectionStore;

class UpdateTimelineTest extends \PHPUnit_Framework_TestCase
{
    /** @var IProjectionStore */
    private $projectionStore;

    /** @var ITimelineMessageRepository */
    private $timelineMessageRepository;

    /** @var UpdateTimeline */
    private $updateTimeline;

    public function setup()
    {
        $this->projectionStore = new InMemoryProjectionStore();
        $this->timelineMessageRepository = new TimelineMessageRepository($this->projectionStore);
        $this->updateTimeline = new UpdateTimeline($this->timelineMessageRepository);
    }

    public function testWhenHandleMessagePublished_ThenTimelineMessageIsSavedForAuthor()
    {
        $messagePublished = new MessagePublished(MessageId::generate(), 'hello', new UserId('clem@mix-it.fr'));

        $this->updateTimeline->handleMessagePublished($messagePublished);

        /** @var TimelineMessage $timelineMessage */
        $timelineMessage = $this->projectionStore->get($messagePublished->getMessageId()->getId(), 'App\Domain\Timeline\TimelineMessage');
        \Assert\that($timelineMessage->getMessageId())->eq($messagePublished->getMessageId());
        \Assert\that($timelineMessage->getOwnerId())->eq($messagePublished->getAuthorId());
        \Assert\that($timelineMessage->getContent())->eq($messagePublished->getContent());
    }

    public function testWhenHandleReplyMessagePublished_ThenTimelineMessageIsSavedForReplier()
    {
        $replyMessagePublished = new ReplyMessagePublished(MessageId::generate(), 'hello too', new UserId('clem@mix-it.fr'), MessageId::generate());

        $this->updateTimeline->handleReplyMessagePublished($replyMessagePublished);

        /** @var TimelineMessage $timelineMessage */
        $timelineMessage = $this->projectionStore->get($replyMessagePublished->getReplyId()->getId(), 'App\Domain\Timeline\TimelineMessage');
        \Assert\that($timelineMessage->getMessageId())->eq($replyMessagePublished->getReplyId());
        \Assert\that($timelineMessage->getOwnerId())->eq($replyMessagePublished->getReplierId());
        \Assert\that($timelineMessage->getContent())->eq($replyMessagePublished->getReplyContent());
    }
}