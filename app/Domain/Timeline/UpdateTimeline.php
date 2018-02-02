<?php

namespace App\Domain\Timeline;

use App\Domain\Messages\MessageQuacked;

class UpdateTimeline
{
    private $repository;

    public function __construct(ITimelineMessageRepository $repository)
    {
        $this->repository = $repository;
    }

    public function handleMessageQuacked(MessageQuacked $messageQuacked): void
    {
        $this->repository->save(new TimelineMessage(
            $messageQuacked->getMessageId(),
            $messageQuacked->getContent(),
            $messageQuacked->getAuthorId()
        ));
    }
}
