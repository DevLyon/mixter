<?php

namespace App\Domain\Timeline;

use App\Domain\Messages\MessagePublished;
use App\Domain\Messages\MessageRepublished;
use App\Domain\Messages\ReplyMessagePublished;

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

    public function handleReplyMessagePublished(ReplyMessagePublished $replyMessagePublished)
    {
        $this->timelineMessageRepository->save(
            new TimelineMessage($replyMessagePublished->getReplyId(), $replyMessagePublished->getReplyContent(), $replyMessagePublished->getReplierId())
        );
    }

    public function handleMessageRepublished(MessageRepublished $messageRepublished)
    {
        $timelineMessage = $this->timelineMessageRepository->getByMessageId($messageRepublished->getMessageId());
        $timelineMessage->incrementNbRepublish();
    }
}