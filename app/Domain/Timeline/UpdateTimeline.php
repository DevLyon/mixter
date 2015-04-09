<?php

namespace App\Domain\Timeline;

use App\Domain\Messages\MessagePublished;

class UpdateTimeline
{
    /**
     * @var ITimelineMessageRepository
     */
    private $timelineMessageRepository;

    public function __construct(ITimelineMessageRepository $timelineMessageRepository)
    {
        $this->timelineMessageRepository = $timelineMessageRepository;
    }

    public function handleMessagePublished(MessagePublished $messagePublished)
    {
        $this->timelineMessageRepository->save(
            new TimelineMessage($messagePublished->getMessageId(), $messagePublished->getContent(), $messagePublished->getAuthorId())
        );
    }
}