<?php

namespace App\Infrastructure\Timeline;

use App\Domain\Messages\MessageId;
use App\Domain\Timeline\ITimelineMessageRepository;
use App\Domain\Timeline\TimelineMessage;
use App\Domain\Timeline\TimelineMessageId;
use App\Infrastructure\IProjectionStore;

class TimelineMessageRepository implements ITimelineMessageRepository
{
    const PROJECTION_TYPE = 'App\Domain\Timeline\TimelineMessage';

    /**
     * @var IProjectionStore
     */
    private $projectionStore;

    public function __construct(IProjectionStore $projectionStore)
    {
        $this->projectionStore = $projectionStore;
    }

    public function getByMessageId(MessageId $messageId)
    {
        return array_filter($this->getAll(), function(TimelineMessage $timelineMessage) use($messageId) {
            return $timelineMessage->getMessageId() == $messageId;
        });
    }

    public function save(TimelineMessage $timelineMessage)
    {
        $timelineMessageId = new TimelineMessageId($timelineMessage->getMessageId(), $timelineMessage->getOwnerId());
        $this->projectionStore->store($timelineMessageId->getId(), $timelineMessage);
    }

    public function getAll()
    {
        return $this->projectionStore->getAll(self::PROJECTION_TYPE);
    }

    public function getByOwnerId($ownerId)
    {
        return array_filter($this->getAll(), function(TimelineMessage $timelineMessage) use($ownerId) {
            return $timelineMessage->getOwnerId() == $ownerId;
        });
    }
}