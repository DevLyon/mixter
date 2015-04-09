<?php

namespace App\Domain\Timeline;

use App\Domain\Messages\MessageQuacked;

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

    public function handleMessageQuacked(MessageQuacked $messageQuacked)
    {
        $this->timelineMessageRepository->save(
            new TimelineMessage($messageQuacked->getMessageId(), $messageQuacked->getContent(), $messageQuacked->getAuthorId())
        );
    }
}