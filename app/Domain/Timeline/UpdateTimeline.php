<?php

namespace App\Domain\Timeline;

use App\Domain\Messages\IMessageProjectionRepository;
use App\Domain\Messages\MessageProjection;
use App\Domain\Messages\MessagePublished;
use App\Domain\Messages\MessageRepublished;
use App\Domain\Messages\ReplyMessagePublished;
use App\Domain\Subscriptions\FolloweeMessagePublished;

class UpdateTimeline
{
    /**
     * @var ITimelineMessageRepository
     */
    private $timelineMessageRepository;

    /**
     * @var IMessageProjectionRepository
     */
    private $messageProjectionRepository;

    public function __construct(ITimelineMessageRepository $timelineMessageRepository, IMessageProjectionRepository $messageProjectionRepository)
    {
        $this->timelineMessageRepository = $timelineMessageRepository;
        $this->messageProjectionRepository = $messageProjectionRepository;
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
        $timelineMessages = $this->timelineMessageRepository->getByMessageId($messageRepublished->getMessageId());
        /** @var TimelineMessage $timelineMessage */
        foreach($timelineMessages as $timelineMessage) {
            $timelineMessage->incrementNbRepublish();
        }
    }

    public function handleFolloweeMessagePublished(FolloweeMessagePublished $followeeMessagePublished)
    {
        /** @var MessageProjection $messageProjection */
        $messageProjection = $this->messageProjectionRepository->getById($followeeMessagePublished->getMessageId());
        $this->timelineMessageRepository->save(new TimelineMessage($messageProjection->getMessageId(), $messageProjection->getContent(), $followeeMessagePublished->getSubscriptionId()->getFollowerId()));
    }
}